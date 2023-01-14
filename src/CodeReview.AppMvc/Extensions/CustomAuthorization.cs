using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CodeReview.AppMvc.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimUsuario(string claimNname, string ClainValue)
        {
            var identity=(ClaimsIdentity) HttpContext.Current.User.Identity;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == claimNname);

            return claim != null && claim.Value.Contains(ClainValue);
        }
    }


    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _claimName;
        private readonly string _claimValue;

        public ClaimsAuthorizeAttribute(string claimName ,string claimValue)
        {
            _claimName= claimName;
            _claimValue= claimValue;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result=new HttpStatusCodeResult((int) HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return CustomAuthorization.ValidarClaimUsuario(_claimName, _claimValue);   
        }
    }
}