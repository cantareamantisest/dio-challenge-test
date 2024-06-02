using Desafio.Dio.Identity.Configuration;
using Desafio.Dio.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Desafio.Dio.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<CustomUser> signInManager,
            UserManager<CustomUser> userManager,
            IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);
            if (result.Succeeded)
                return await GenerateCredencials(request.Email);

            var loginResponse = new LoginResponse();
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    loginResponse.AddError("Essa conta está bloqueada");
                else if (result.IsNotAllowed)
                    loginResponse.AddError("Essa conta não tem permissão para fazer login");
                else if (result.RequiresTwoFactor)
                    loginResponse.AddError("É necessário confirmar o login no seu segundo fator de autenticação");
                else
                    loginResponse.AddError("Usuário ou senha estão incorretos");
            }

            return loginResponse;
        }

        public async Task<LoginResponse> LoginWithoutPassword(string userId)
        {
            var loginResponse = new LoginResponse();
            var user = await _userManager.FindByIdAsync(userId);

            if (await _userManager.IsLockedOutAsync(user))
                loginResponse.AddError("Essa conta está bloqueada");
            else if (!await _userManager.IsEmailConfirmedAsync(user))
                loginResponse.AddError("Essa conta precisa confirmar seu e-mail antes de realizar o login");

            if (loginResponse.Success)
                return await GenerateCredencials(user.Email);

            return loginResponse;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var customUser = new CustomUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(customUser, request.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(customUser, false);

            var registerResponse = new RegisterResponse(result.Succeeded);
            if (!result.Succeeded && result.Errors.Count() > 0)
                registerResponse.AddErrors(result.Errors.Select(r => r.Description));

            return registerResponse;
        }

        private async Task<LoginResponse> GenerateCredencials(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var accessTokenClaims = await GetClaims(user, addClaimsUser: true);
            var refreshTokenClaims = await GetClaims(user, addClaimsUser: false);

            var dataExpiracaoAccessToken = DateTime.UtcNow.AddSeconds(_jwtOptions.AccessTokenExpiration);
            var dataExpiracaoRefreshToken = DateTime.UtcNow.AddSeconds(_jwtOptions.RefreshTokenExpiration);

            var accessToken = GenerateToken(accessTokenClaims, dataExpiracaoAccessToken);
            var refreshToken = GenerateToken(refreshTokenClaims, dataExpiracaoRefreshToken);

            return new LoginResponse
            (
                success: true,
                accessToken: accessToken,
                refreshToken: refreshToken
            );
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime dateExpires)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dateExpires,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> GetClaims(CustomUser user, bool addClaimsUser)
        {
            var claims = new List<Claim>(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            });

            if (addClaimsUser)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(userClaims);

                foreach (var role in roles)
                    claims.Add(new Claim("role", role));
            }

            return claims;
        }
    }
}