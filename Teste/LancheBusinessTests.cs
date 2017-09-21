using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Teste
{
    [TestClass]
    public class LancheBusinessTests : TestsBase
    {
        // cobertura de teste das promoções

        [TestMethod]
        public void DeveCairNaPromocaoLight()
        {
            var ingredienteAlface = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Alface);
            int qtdeDePorcao = 1;

            var porcao = new Porcao
            {
                IngredienteId = ingredienteAlface.Id,
                Ingrediente = ingredienteAlface,
                Quantidade = qtdeDePorcao
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcao: porcao);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.AreEqual(1, promocoes.Count, "Só deveria ter uma única promoção");
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.Light), "Deveria ter a promoção");
        }

        [TestMethod]
        public void NaoDeveCairNaPromocaoLightPoisPossuiBacon()
        {
            var ingredienteAlface = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Alface);
            var ingredienteBacon = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Bacon);
            int qteDePorcao = 1;

            var porcoes = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ingredienteAlface,
                    Quantidade = qteDePorcao
                },
                new Porcao
                {
                    Ingrediente = ingredienteBacon,
                    Quantidade = qteDePorcao
                }
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcoes: porcoes);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.IsTrue(promocoes.Count == 0);
        }

        [TestMethod]
        public void DeveCairNaPromocaoMuitaCarne()
        {
            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qtdeDePorcao = 5;

            var porcao = new Porcao
            {
                Ingrediente = ingredienteCarne,
                Quantidade = qtdeDePorcao
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcao: porcao);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.AreEqual(1, promocoes.Count, "Só deveria ter uma única promoção");
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.MuitaCarne), "Deveria ter a promoção");
        }

        [TestMethod]
        public void NaoDeveCairNaPromocaoMuitaCarnePoisPossuiApenas2Porcao()
        {
            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qtdeDePorcao = 2;

            var porcao = new Porcao
            {
                Ingrediente = ingredienteCarne,
                Quantidade = qtdeDePorcao
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcao: porcao);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.IsTrue(promocoes.Count == 0);
        }

        [TestMethod]
        public void DeveCairNaPromocaoMuitoQueijo()
        {
            var ingredienteQueijo = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Queijo);
            int qtdeDePorcao = 4;

            var porcao = new Porcao
            {
                Ingrediente = ingredienteQueijo,
                Quantidade = qtdeDePorcao
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcao: porcao);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.AreEqual(1, promocoes.Count, "Só deveria ter uma única promoção");
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.MuitoQueijo), "Deveria ter a promoção");
        }

        [TestMethod]
        public void NaoDeveCairNaPromocaoMuitoQueijoPoisPossuiApenas2Porcao()
        {
            var ingredienteQueijo = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Queijo);
            int qtdeDePorcao = 2;

            var porcao = new Porcao
            {
                Ingrediente = ingredienteQueijo,
                Quantidade = qtdeDePorcao
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcao: porcao);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.IsTrue(promocoes.Count == 0);
        }

        [TestMethod]
        public void DeveCairNaPromocaoMuitoQueijoEMuitaCarneELight()
        {
            var ingredienteAlface = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Alface);
            int qtdedePorcaoDoIngredienteAlcafe = 1;

            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qtdedePorcaoDoIngredienteCarne = 3;

            var ingredienteQueijo = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Queijo);
            int qtdedePorcaoDoIngredienteQueijo = 4;

            var porcoes = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ingredienteAlface,
                    Quantidade = qtdedePorcaoDoIngredienteAlcafe
                },
                new Porcao
                {
                    Ingrediente = ingredienteCarne,
                    Quantidade = qtdedePorcaoDoIngredienteCarne
                },
                new Porcao
                {
                    Ingrediente = ingredienteQueijo,
                    Quantidade = qtdedePorcaoDoIngredienteQueijo
                },
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcoes: porcoes);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.AreEqual(3, promocoes.Count, "Deveria ter as 3 promoções");
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.Light));
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.MuitaCarne));
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.MuitoQueijo));
        }

        [TestMethod]
        public void DeveCairNaPromocaoMuitoQueijoELight()
        {
            var ingredienteAlface = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Alface);
            int qtdedePorcaoDoIngredienteAlcafe = 1;

            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qtdedePorcaoDoIngredienteCarne = 1;

            var ingredienteQueijo = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Queijo);
            int qtdedePorcaoDoIngredienteQueijo = 4;

            var porcoes = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ingredienteAlface,
                    Quantidade = qtdedePorcaoDoIngredienteAlcafe
                },
                new Porcao
                {
                    Ingrediente = ingredienteCarne,
                    Quantidade = qtdedePorcaoDoIngredienteCarne
                },
                new Porcao
                {
                    Ingrediente = ingredienteQueijo,
                    Quantidade = qtdedePorcaoDoIngredienteQueijo
                },
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcoes: porcoes);

            var promocoes = BusinessFacade.Lanche.ListaDePromocoesQueSeraoAplicadas(lanche);

            Assert.AreEqual(2, promocoes.Count, "Deveria ter as 3 promoções");
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.Light));
            Assert.IsTrue(!promocoes.Any(p => p == Enumeracao.EPromocao.MuitaCarne));
            Assert.IsTrue(promocoes.Any(p => p == Enumeracao.EPromocao.MuitoQueijo));
        }

        // valor dos lanches e regra de cálculo

        [TestMethod]
        public void DeveCobrarApenas2PorcaoDeCarneParaLanchesComMuitaCarne()
        {
            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qteDePorcoesDeCarneNoLanche = 3;
            int qtdeDePorcoesDeCarneParaSerCobrada = 2;
            decimal valorASerCobradoPeloLanche = qtdeDePorcoesDeCarneParaSerCobrada * ingredienteCarne.Valor;

            var porcao = new Porcao
            {
                Ingrediente = ingredienteCarne,
                Quantidade = qteDePorcoesDeCarneNoLanche
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcao: porcao);

            BusinessFacade.Lanche.Incluir(lanche);

            Assert.AreEqual(qteDePorcoesDeCarneNoLanche, lanche.Porcoes.Sum(p => p.Quantidade));
            Assert.AreEqual(valorASerCobradoPeloLanche, lanche.Valor);
        }

        [TestMethod]
        public void DeveConcederDescontoDe10PercentualDoValorDoLancheQuandoAplicarPromocaoLight()
        {
            var ingredienteAlface = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Alface);
            int qtdedePorcaoDoIngredienteAlcafe = 1;

            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qtdedePorcaoDoIngredienteCarne = 2;

            var ingredienteQueijo = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Queijo);
            int qtdedePorcaoDoIngredienteQueijo = 2;

            var porcoes = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ingredienteAlface,
                    Quantidade = qtdedePorcaoDoIngredienteAlcafe
                },
                new Porcao
                {
                    Ingrediente = ingredienteCarne,
                    Quantidade = qtdedePorcaoDoIngredienteCarne
                },
                new Porcao
                {
                    Ingrediente = ingredienteQueijo,
                    Quantidade = qtdedePorcaoDoIngredienteQueijo
                }
            };

            var valorASerCobradoPeloLanche = BusinessFacade.Lanche.ValorDeTodosOsIngredientes(porcoes) * 0.9M; //desconto de 10%
            var qtdeDePorcoesDoLanche = qtdedePorcaoDoIngredienteAlcafe + qtdedePorcaoDoIngredienteCarne + qtdedePorcaoDoIngredienteQueijo;

            var lanche = FabricaDeDados.MontarUmLanche(porcoes: porcoes);

            BusinessFacade.Lanche.Incluir(lanche);

            Assert.AreEqual(qtdeDePorcoesDoLanche, lanche.Porcoes.Sum(p => p.Quantidade));
            Assert.AreEqual(valorASerCobradoPeloLanche, lanche.Valor);
        }

        [TestMethod]
        public void DeveCobrarApenas15CarneECobrar11QueijoNoLancheComPromocaoMuitoQueijoEMuitaCarne()
        {
            var ingredienteCarne = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.HamburguerDeCarne);
            int qtdedePorcaoDoIngredienteCarne = 22;
            int qtdeDePorcaoParaCobrarDeCarne = 15;

            var ingredienteQueijo = BusinessFacade.Ingrediente.BuscarIngrediente(Enumeracao.ETipoDeIngrediente.Queijo);
            int qtdedePorcaoDoIngredienteQueijo = 16;
            int qtdeDePorcaoParaCobrarDeQueijo = 11;

            var valorASerCobradoPeloLanche = (ingredienteCarne.Valor * qtdeDePorcaoParaCobrarDeCarne) + (ingredienteQueijo.Valor * qtdeDePorcaoParaCobrarDeQueijo);
            var qtdeDePorcoesDoLanche = qtdedePorcaoDoIngredienteCarne + qtdedePorcaoDoIngredienteQueijo;

            var porcoes = new List<Porcao>
            {
                new Porcao
                {
                    Ingrediente = ingredienteCarne,
                    Quantidade = qtdedePorcaoDoIngredienteCarne
                },
                new Porcao
                {
                    Ingrediente = ingredienteQueijo,
                    Quantidade = qtdedePorcaoDoIngredienteQueijo
                }
            };

            var lanche = FabricaDeDados.MontarUmLanche(porcoes: porcoes);

            BusinessFacade.Lanche.Incluir(lanche);

            Assert.AreEqual(qtdeDePorcoesDoLanche, lanche.Porcoes.Sum(p => p.Quantidade));
            Assert.AreEqual(valorASerCobradoPeloLanche, lanche.Valor);
        }
    }
}
