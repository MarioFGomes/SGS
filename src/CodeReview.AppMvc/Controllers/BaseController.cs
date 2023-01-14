using CodeReview.Business.Core.Notificacoes;
using System.Web.Mvc;

namespace CodeReview.AppMvc.Controllers
{
    public class BaseController: Controller
    {
        private readonly INotificador _notificador;
        public BaseController(INotificador notificador)
        {
            _notificador=notificador;
        }

        protected bool OperacaoValida()
        {
            if(!_notificador.TemNoficacao()) return true;
            var notificacoes=_notificador.ObterNoficacao();
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));
            return false;
        }
    }
}