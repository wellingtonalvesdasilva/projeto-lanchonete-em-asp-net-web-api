using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Arquitetura.Erro
{
    [Serializable]
    public class BusinessException : Exception
    {
        protected string campoAssociado;

        protected IList<KeyValuePair<string, string>> excecoes = new List<KeyValuePair<string, string>>();

        public BusinessException(string mensagem) : base(mensagem)
        {
        }

        public BusinessException(string campoAssociado, string mensagem) : base(mensagem)
        {
            this.campoAssociado = campoAssociado;

            excecoes = new List<KeyValuePair<string, string>>();
            excecoes.Add(new KeyValuePair<string, string>(campoAssociado, mensagem));
        }

        public BusinessException(IList<KeyValuePair<string, string>> excecoes) : base("Campos obrigatórios não informados: " + ObterChaves(excecoes))
        {
            this.excecoes = excecoes;
        }

        public BusinessException(List<ExcecaoCampoObrigatorio> excecoes) : base("Campos obrigatórios não informados: " + ObterChaves(excecoes))
        {
            this.excecoes = ConverterLista(excecoes);
        }

        public BusinessException(Type entityType, IList<KeyValuePair<string, string>> excecoes)
            : base("Campos obrigatórios não informados: " + ObterChaves(excecoes))
        {
            this.excecoes = excecoes;
        }

        public BusinessException()
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static IList<KeyValuePair<string, string>> ConverterLista(IList<ExcecaoCampoObrigatorio> excecoes)
        {
            var kvExcecoes = new List<KeyValuePair<string, string>>();
            foreach (var excecao in excecoes)
                kvExcecoes.Add(new KeyValuePair<string, string>(excecao.campoAssociado, excecao.Message));
            return kvExcecoes;
        }

        private static string ObterChaves(IList<ExcecaoCampoObrigatorio> excecoes)
        {
            return ObterChaves(ConverterLista(excecoes));
        }

        private static string ObterChaves(IList<KeyValuePair<string, string>> excecoes)
        {
            StringBuilder result = new StringBuilder();
            foreach (var excecao in excecoes)
                result.Append("</br>- " + excecao.Value);
            return result.ToString();
        }

        public IList<KeyValuePair<string, string>> Excecoes
        {
            get { return excecoes; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("campoAssociado", campoAssociado);
            foreach (var excecao in excecoes)
                info.AddValue("excecao_" + excecao.Key, excecao.Value);
        }
    }

    public static class IListExtensionForBoException
    {
        public static void Add(this IList<KeyValuePair<string, string>> excecoes, string campoAssociado, string mensagem)
        {
            excecoes.Add(new KeyValuePair<string, string>(campoAssociado, mensagem));
        }

        public static void Add(this IList<KeyValuePair<string, string>> excecoes, Type propriedadeAssociada, string mensagem)
        {
            excecoes.Add(new KeyValuePair<string, string>(propriedadeAssociada.Name, mensagem));
        }
    }

}
