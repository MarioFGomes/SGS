using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeReview.AppMvc.Extensions
{
    public static class RazorExtensions
    {
        public static bool PermitirExibicao(this WebViewPage page,string ClaimName,string ClaimValue)
        {
            return CustomAuthorization.ValidarClaimUsuario(ClaimName,ClaimValue);
        }

        public static MvcHtmlString PermitirExibicao(this MvcHtmlString value, string ClaimName, string ClaimValue)
        {
            return CustomAuthorization.ValidarClaimUsuario(ClaimName, ClaimValue) ? value : MvcHtmlString.Empty;
        }

        public static string FormatarDocumento(this WebViewPage page,int tipopessoa,string documento)
        {
            return tipopessoa == 1
                ? Convert.ToInt64(documento).ToString(@"000\.000\.000\-00")
                : Convert.ToInt64(documento).ToString(@"00\.000\.000\/0000\-00");
        }

        public static bool ExibirNaURL(this WebViewPage value, Guid Id)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var urlTarget= urlHelper.Action("Edit", "Fornecedores",new {id=Id});
            var urlTarget2 = urlHelper.Action("ObterEndereco", "Fornecedores", new { id = Id });
            var urlEmUso = HttpContext.Current.Request.Path;

            return urlTarget== urlEmUso || urlTarget2==urlEmUso;
        }
    }
}