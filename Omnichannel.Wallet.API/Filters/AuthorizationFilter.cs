using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Omnichannel.Wallet.API.Messages;

namespace Omnichannel.Wallet.API.Filters
{
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                context.ActionArguments.TryGetValue("request", out object inputParam);

                if (inputParam == null) throw new InvalidOperationException();

                if (inputParam is IMultitenantOperation multitenantOperation)
                {
                    var company = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "company")?.Value;

                    if (string.IsNullOrEmpty(company)) context.Result = new UnauthorizedResult();

                    multitenantOperation.Company = company;
                }

                if (inputParam is ISecurityOperation securityOperation)
                {
                    var cpf = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

                    if (string.IsNullOrEmpty(cpf)) context.Result = new UnauthorizedResult();

                    securityOperation.CPF = cpf;
                }

                await next();
            }
            catch (Exception)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
