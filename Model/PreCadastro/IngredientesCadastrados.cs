using System.Collections.Generic;
using Util;

namespace Model.PreCadastro
{
    public class IngredientesCadastrados
    {
        // Fixo no Backend, se necessário alterar aqui os valores dos ingredientes

        public static List<Ingrediente> Disponiveis()
        {
            var ingredientes = new List<Ingrediente>
            {
                new Ingrediente
                {
                    Id = (long)Enumeracao.ETipoDeIngrediente.Alface,
                    Nome = "Alface",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = 0.4M
                },
                new Ingrediente
                {
                    Id = (long)Enumeracao.ETipoDeIngrediente.Bacon,
                    Nome = "Bacon",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = 2M
                },
                new Ingrediente
                {
                    Id = (long)Enumeracao.ETipoDeIngrediente.HamburguerDeCarne,
                    Nome = "Hambúrguer de carne",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = 3M
                },
                new Ingrediente
                {
                    Id = (long)Enumeracao.ETipoDeIngrediente.Ovo,
                    Nome = "Ovo",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = 0.8M
                },
                new Ingrediente
                {
                    Id = (long)Enumeracao.ETipoDeIngrediente.Queijo,
                    Nome = "Queijo",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = 1.5M
                }
            };

            return ingredientes;
        }
    }
}
