using System;
using System.Collections.Generic;

namespace Arquitetura.Erro
{
    [Serializable]
    public class ExcecaoCampoObrigatorio : BusinessException
    {
        public ExcecaoCampoObrigatorio(string nomeDoCampo, string mensagem) : base(nomeDoCampo, mensagem)
        {
        }

        public ExcecaoCampoObrigatorio(string mensagem) : base(string.Empty, mensagem)
        {
        }

        public ExcecaoCampoObrigatorio(IList<KeyValuePair<string, string>> excecoes) : base(excecoes)
        {
        }

        public ExcecaoCampoObrigatorio(Type entityType, IList<KeyValuePair<string, string>> excecoes) : base(entityType, excecoes)
        {
        }

        public ExcecaoCampoObrigatorio(List<ExcecaoCampoObrigatorio> excecoes) : base(excecoes)
        {
        }
    }
}
