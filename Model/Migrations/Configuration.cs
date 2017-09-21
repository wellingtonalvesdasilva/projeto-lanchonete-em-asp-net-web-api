namespace Model.Migrations
{
    using PreCadastro;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Model.LanchoneteEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LanchoneteEntities context)
        {
            var ingredientes = IngredientesCadastrados.Disponiveis();

            foreach (var ingrediente in ingredientes)
                context.Ingrediente.AddOrUpdate(p => p.Nome, ingrediente);

            var lanches = LanchesCadastrados.Disponiveis(context.Ingrediente.ToList());

            foreach (var lanche in lanches)
                context.Lanche.AddOrUpdate(p => p.Nome, lanche);

            context.SaveChanges();

        }
    }
}
