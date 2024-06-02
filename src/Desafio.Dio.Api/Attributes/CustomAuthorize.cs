using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Desafio.Dio.Api.Attributes
{
    public class CustomAuthorize
    {
        public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }

        public class ClaimsAuthorizeAttribute : TypeFilterAttribute
        {
            public ClaimsAuthorizeAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter)) =>
                Arguments = new object[] { new Claim(claimType, claimValue) };
        }

        public class ClaimRequirementFilter : IAuthorizationFilter
        {
            readonly Claim _claim;

            public ClaimRequirementFilter(Claim claim) =>
                _claim = claim;

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User as ClaimsPrincipal;

                if (user == null || !user.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (!ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
                    context.Result = new ForbidResult();

                if (!user.HasClaim(_claim.Type, _claim.Value))
                    context.Result = new ForbidResult();
            }
        }
    }
}
