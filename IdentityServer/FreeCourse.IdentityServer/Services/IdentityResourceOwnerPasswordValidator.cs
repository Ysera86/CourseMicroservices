using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    /// <summary>
    ///  token almak çin istek yapıldığında email şifre kontrolü yapan IResourceOwnerPasswordValidator olduğundan, artık Resource owner credential grant tipinde bir istek yapıldığında benim IdentityResourceOwnerPasswordValidator devreye girecek. Startupta DI da geçtik. 
    /// </summary>
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userExists = await _userManager.FindByEmailAsync(context.UserName);
            if (userExists != null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Wrong email or password" });

                context.Result.CustomResponse=errors;

                return;
            }

            var passwordControl = await _userManager.CheckPasswordAsync(userExists, context.Password);
            if (!passwordControl)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "Wrong email or password" });

                context.Result.CustomResponse = errors;

                return;
            }

            //genel hatalar verdik ki şifre yanlışsa demek mail doğru mesajo vermesin kötü amaçlı kişilere diye

            context.Result = new GrantValidationResult(userExists.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            // token üretecek
        }   
    }
}
