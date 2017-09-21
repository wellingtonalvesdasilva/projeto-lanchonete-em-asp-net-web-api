using System.Collections.Generic;
using System.Linq;
using Util;

namespace Model.PreCadastro
{
    public class LanchesCadastrados
    {
        public static List<Lanche> Disponiveis(List<Ingrediente> ingredientes)
        {
            var alface = ingredientes.SingleOrDefault(i => i.Id == (int)Enumeracao.ETipoDeIngrediente.Alface);
            var bacon = ingredientes.SingleOrDefault(i => i.Id == (int)Enumeracao.ETipoDeIngrediente.Bacon);
            var hamburguerDeCarne = ingredientes.SingleOrDefault(i => i.Id == (int)Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            var ovo = ingredientes.SingleOrDefault(i => i.Id == (int)Enumeracao.ETipoDeIngrediente.Ovo);
            var queijo = ingredientes.SingleOrDefault(i => i.Id == (int)Enumeracao.ETipoDeIngrediente.Queijo);

            //Bacon, hambúrguer de carne e queijo
            var porcoesXBacon = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = bacon,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = hamburguerDeCarne,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = queijo,
                    Quantidade = 1
                }
            };

            //Hambúrguer de carne e queijo
            var porcoesXBurger = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = hamburguerDeCarne,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = queijo,
                    Quantidade = 1
                }
            };

            //Ovo, hambúrguer de carne e queijo
            var porcoesXEgg = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ovo,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = hamburguerDeCarne,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = queijo,
                    Quantidade = 1
                }
            };

            //Ovo, bacon, hambúrguer de carne e queijo
            var porcoesXEggBacon = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ovo,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = bacon,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = hamburguerDeCarne,
                    Quantidade = 1
                },
                new Porcao
                {
                    Ingrediente = queijo,
                    Quantidade = 1
                }
            };

            var lanches = new List<Lanche>
            {
                new Lanche
                {
                    
                    Nome = "X-Bacon",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = ValorDeTodosOsIngredientes(porcoesXBacon),
                    Porcoes = porcoesXBacon
                },
                new Lanche
                {
                    
                    Nome = "X-Burger",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = ValorDeTodosOsIngredientes(porcoesXBurger),
                    Porcoes = porcoesXBurger
                },
                new Lanche
                {
                    Nome = "X-Egg",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = ValorDeTodosOsIngredientes(porcoesXEgg),
                    Porcoes = porcoesXEgg
                },
                new Lanche
                {
                    Nome = "X-Egg Bacon",
                    Status = (int)Enumeracao.ESituacao.Ativo,
                    Valor = ValorDeTodosOsIngredientes(porcoesXEggBacon),
                    Porcoes = porcoesXEggBacon
                }
            };

            return lanches;
        }

        private static decimal ValorDeTodosOsIngredientes(IEnumerable<Porcao> porcoes)
        {
            return porcoes.Sum(p => p.Ingrediente.Valor * p.Quantidade);
        }
    }
}
