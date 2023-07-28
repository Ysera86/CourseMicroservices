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
    }
}
