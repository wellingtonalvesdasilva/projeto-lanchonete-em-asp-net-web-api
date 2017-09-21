namespace Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class EstruturaInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredientes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lanches",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Porcaos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Quantidade = c.Int(nullable: false),
                        Ingrediente_Id = c.Long(nullable: false),
                        Lanche_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredientes", t => t.Ingrediente_Id, cascadeDelete: true)
                .ForeignKey("dbo.Lanches", t => t.Lanche_Id)
                .Index(t => t.Ingrediente_Id)
                .Index(t => t.Lanche_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Porcaos", "Lanche_Id", "dbo.Lanches");
            DropForeignKey("dbo.Porcaos", "Ingrediente_Id", "dbo.Ingredientes");
            DropIndex("dbo.Porcaos", new[] { "Lanche_Id" });
            DropIndex("dbo.Porcaos", new[] { "Ingrediente_Id" });
            DropTable("dbo.Porcaos");
            DropTable("dbo.Lanches");
            DropTable("dbo.Ingredientes");
        }
    }
}
