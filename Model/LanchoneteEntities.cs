using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Model
{
    public class LanchoneteEntities : DbContext
    {
        public LanchoneteEntities(string connectionString) : base(connectionString)
        { }

        public LanchoneteEntities() : base("name=LanchoneteEntities")
        { }

        public LanchoneteEntities(DbConnection connection) : base(connection, true)
        { }

        public DbSet<Ingrediente> Ingrediente { get; set; }
        public DbSet<Lanche> Lanche { get; set; }
        public DbSet<Porcao> Porcao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //remove pluralizacao
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //padroniza todos os campos string para varchar(60)
            //modelBuilder.Properties<string>()
            //    .Configure(p => p.HasColumnType("varchar"));
            //modelBuilder.Properties<string>()
            //    .Configure(p => p.HasMaxLength(60));
        }
    }
}
