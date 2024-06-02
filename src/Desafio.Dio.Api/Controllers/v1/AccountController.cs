using Desafio.Dio.Api.Shared;
using Desafio.Dio.Identity.Models;
using Desafio.Dio.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Desafio.Dio.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : ApiControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Cadastro de usuário.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="registerUser">Dados de cadastro do usuário</param>
        /// <returns></returns>
        /// <response code="200">Usuário criado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Retorna erros caso ocorram</response>
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _identityService.Register(registerUser);
            if (result.Success)
                return Ok(result);
            else if (result.Errors.Count > 0)
            {
                var problemDetails = new CustomProblemDetails(HttpStatusCode.BadRequest, Request, errors: result.Errors);
                return BadRequest(problemDetails);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Login do usuário via usuário/senha.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userLogin">Dados de login do usuário</param>
        /// <returns></returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="401">Erro caso usuário não esteja autorizado</response>
        /// <response code="500">Retorna erros caso ocorram</response>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _identityService.Login(userLogin);
            if (result.Success)
                return Ok(result);

            return Unauthorized();
        }

        /// <summary>
        /// Login do usuário via refresh token.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="401">Erro caso usuário não esteja autorizado</response>
        /// <response code="500">Retorna erros caso ocorram</response>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPost("refresh-login")]
        public async Task<ActionResult<LoginResponse>> RefreshLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return BadRequest();

            var resultado = await _identityService.LoginWithoutPassword(userId);
            if (resultado.Success)
                return Ok(resultado);

            return Unauthorized();
        }
    }
}
