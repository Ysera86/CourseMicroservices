using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor; // Bunu merkezileştirdik, bunun içinden contexte, onun içinden de User.Claims nesnesi üzerinden sub yani subject yani tokenın kimin için olduğu (kullanıcı) bilgisine erişicem. Bunu tüm apilarda (resource owner credentials (üyelik) grant tipli) kullanıcam, hepsinde ayyrı olmaması için Shared içine aldık, bunu tabi DI yapablmek için, bunu kullanacak tüm APIlerde servislere DI olarak geçmeliyim.  örn :  Basket

        public SharedIdentityService(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }

        //public string GetUserId => _httpContextAccessor.HttpContext.User.Claims.Where(x =>x.Type == "sub").FirstOrDefault().Value;
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        // claimlerde sub diye değil {http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: diye geldi : maplemesini değiştirmem lazım ki sub diye gelsin. Basket Program içinde : JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
        // {http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: 8f771a64-0d10-4eb7-b733-4128fef8bf9a} 
    }
}
