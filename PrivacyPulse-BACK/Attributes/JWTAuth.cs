using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PrivacyPulse_BACK.Services;
using System.Security.Claims;
using PrivacyPulse_BACK.Services;

namespace PrivacyPulse_BACK.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JWTAuth : Attribute, IAuthorizationFilter
    {
        private readonly JWTService jwtService;
        private readonly string secret = "8937239492972280";

        public JWTAuth()
        {
            jwtService = new JWTService(secret);
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var token = context.HttpContext.Request.Headers["Bearer"];

            if (token.Count < 0)
            {
                Unauthorized(context);
            }
            else
            {
                if (jwtService.ValidateAndReadJWT(token.FirstOrDefault(), out var decodedToken))
                {
                    context.HttpContext.Items.Add("user", int.Parse(decodedToken.Claims.First(x => x.Type == "user").Value));
                }
                else
                {
                    Unauthorized(context);
                }
            }
        }

        private void Unauthorized(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
