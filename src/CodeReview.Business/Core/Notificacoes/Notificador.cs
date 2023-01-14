using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Business.Core.Notificacoes
{
    public class Notificador : INotificador
    {
        private List<Notificacao> _notificacoes;  

        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }
        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public List<Notificacao> ObterNoficacao()
        {
            return _notificacoes;
        }

        public bool TemNoficacao()
        {
           return _notificacoes.Any();
        }
    }
}
