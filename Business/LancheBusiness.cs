using Arquitetura;
using Arquitetura.Business;
using Arquitetura.Erro;
using Model;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Business
{
    public class LancheBusiness : BusinessCrudGeneric<LanchoneteEntities, Lanche>
    {
        const int PERCENTUAL_DE_DESCONTO_PARA_PROMOCAO_LIGHT = 10;
        const int QTDE_MINIMA_DE_PORCOES_PARA_PAGAR_MENOS_UM = 3;

        protected override void ValidarRegrasDeNegocio(Lanche lanche, ECrudOperacao operacao)
        {
            if (operacao == ECrudOperacao.Criar)
            {
                if (repository.GetUnique(r => r.Nome.ToLower() == lanche.Nome.ToLower()) != null)
                    throw new BusinessException("Esse lanche já está cadastrado");
            }
        }

        protected override void ValidarCamposObrigatorios(Lanche lanche, IList<KeyValuePair<string, string>> excecoesAnotadas, ECrudOperacao crudAction)
        {
            if (lanche.Porcoes == null || lanche.Porcoes.Count == 0)
                excecoesAnotadas.Add(string.Empty, "Adicione pelo menos uma porção de ingrediente ao lanche");
            else
            {
                if(lanche.Porcoes.Any(p => p.Quantidade <= 0))
                    excecoesAnotadas.Add(string.Empty, "Não pode adicionar a quantidade de zero ingredientes à porção");

                //poderia criar uma regra para não aceitar o mesmo ingrediente várias vezes, ou seja só pode alterar a quantidade
            }
        }

        private decimal AplicarRegraDeCalculoDePrecoDoLanche(Lanche lanche)
        {
            var porcoes = lanche.Porcoes.ToList();

            //ordenado as promoçoes de acordo com sua aplicação
            var promocoesASeremAplicadas = ListaDePromocoesQueSeraoAplicadas(lanche).OrderBy(p => (int)p);

            decimal percentualDeDescontoASerAplicado = 0; //0% de desconto
            decimal valorDeDescontoDePorcoesNaoCobrada = 0;

            foreach (var promocao in promocoesASeremAplicadas)
            {
                if(promocao == Enumeracao.EPromocao.MuitoQueijo || promocao == Enumeracao.EPromocao.MuitaCarne)
                {
                    long idDoIngrediente = 0;
                    int qtdeDeIngredientesParaNaoSerCobrado = 0;

                    if (promocao == Enumeracao.EPromocao.MuitoQueijo)
                    {
                        idDoIngrediente = (long)Enumeracao.ETipoDeIngrediente.Queijo;
                        var porcao = porcoes.SingleOrDefault(c => c.Ingrediente.Id == idDoIngrediente);
                        qtdeDeIngredientesParaNaoSerCobrado = porcao.Quantidade / QTDE_MINIMA_DE_PORCOES_PARA_PAGAR_MENOS_UM;
                        valorDeDescontoDePorcoesNaoCobrada += qtdeDeIngredientesParaNaoSerCobrado * porcao.Ingrediente.Valor;
                    }
                    else if (promocao == Enumeracao.EPromocao.MuitaCarne)
                    {
                        idDoIngrediente = (long)Enumeracao.ETipoDeIngrediente.HamburguerDeCarne;
                        var porcao = porcoes.SingleOrDefault(c => c.Ingrediente.Id == idDoIngrediente);
                        qtdeDeIngredientesParaNaoSerCobrado = porcao.Quantidade / QTDE_MINIMA_DE_PORCOES_PARA_PAGAR_MENOS_UM;
                        valorDeDescontoDePorcoesNaoCobrada += qtdeDeIngredientesParaNaoSerCobrado * porcao.Ingrediente.Valor;
                    }
                }

                if (promocao == Enumeracao.EPromocao.Light)
                    percentualDeDescontoASerAplicado = PERCENTUAL_DE_DESCONTO_PARA_PROMOCAO_LIGHT; //10% de desconto
            }

            var valorDoLanche = ValorDeTodosOsIngredientes(porcoes) - valorDeDescontoDePorcoesNaoCobrada;
            var valorASerDescontado = valorDoLanche * (percentualDeDescontoASerAplicado / 100);
            return valorDoLanche - valorASerDescontado;
        }

        public List<Enumeracao.EPromocao> ListaDePromocoesQueSeraoAplicadas(Lanche lanche)
        {
            var porcoes = lanche.Porcoes.ToList();
            var promocoesAplicadas = new List<Enumeracao.EPromocao>();

            if (porcoes.Any(p => p.Ingrediente.Id == (long)Enumeracao.ETipoDeIngrediente.Alface) &&
                !porcoes.Any(p => p.Ingrediente.Id == (long)Enumeracao.ETipoDeIngrediente.Bacon))
                promocoesAplicadas.Add(Enumeracao.EPromocao.Light);

            var porcaoDeCarne = porcoes.SingleOrDefault(p => p.Ingrediente.Id == (long)Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            if (porcaoDeCarne != null && porcaoDeCarne.Quantidade >= QTDE_MINIMA_DE_PORCOES_PARA_PAGAR_MENOS_UM)
                promocoesAplicadas.Add(Enumeracao.EPromocao.MuitaCarne);

            var porcaoDeQueijo = porcoes.SingleOrDefault(p => p.Ingrediente.Id == (long)Enumeracao.ETipoDeIngrediente.Queijo);
            if (porcaoDeQueijo != null && porcaoDeQueijo.Quantidade >= QTDE_MINIMA_DE_PORCOES_PARA_PAGAR_MENOS_UM)
                promocoesAplicadas.Add(Enumeracao.EPromocao.MuitoQueijo);

            return promocoesAplicadas;
        }

        public decimal ValorDeTodosOsIngredientes(IEnumerable<Porcao> porcoes)
        {
            return porcoes.Sum(p => p.Ingrediente.Valor * p.Quantidade);
        }

        protected override Lanche IncluirModel(Lanche lanche, bool salvarAlteracoes = true)
        {
            lanche.Valor = AplicarRegraDeCalculoDePrecoDoLanche(lanche);
            return base.IncluirModel(lanche, salvarAlteracoes);
        }
    }
}
