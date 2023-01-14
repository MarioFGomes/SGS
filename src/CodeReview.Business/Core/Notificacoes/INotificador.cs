using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Business.Core.Notificacoes
{
    public interface INotificador
    {
        bool TemNoficacao();
        List<Notificacao> ObterNoficacao();

        void Handle (Notificacao notificacao);
    }
}
