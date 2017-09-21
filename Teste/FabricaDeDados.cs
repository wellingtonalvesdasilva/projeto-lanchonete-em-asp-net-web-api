using Facade;
using Model;
using Model.PreCadastro;
using System.Collections.Generic;
using Util;

namespace Teste
{
    public class FabricaDeDados
    {
        private BusinessFacade BusinessFacade => BusinessFacade.GetInstance();
        private RepositoryFacade RepositoryFacade => RepositoryFacade.GetInstance();

        internal void IncluirTodosIngredientesDisponiveis()
        {
            var ingredientes = IngredientesCadastrados.Disponiveis();

            foreach (var ingrediente in ingredientes)
                RepositoryFacade.Ingrediente.Create(ingrediente);
        }

        public Lanche MontarUmLanche(string nomeDoLanche = "X-Tudo", List<Porcao> porcoes = null, Porcao porcao = null)
        {
            var lanche = new Lanche
            {
                Nome = nomeDoLanche,
                Status = (int)Enumeracao.ESituacao.Ativo,
                Valor = 0
            };

            if(porcoes != null)
                foreach (var porcaoA in porcoes)
                    lanche.Porcoes.Add(porcaoA);

            if(porcao != null)
                lanche.Porcoes.Add(porcao);

            return lanche;
        }
    }
}
