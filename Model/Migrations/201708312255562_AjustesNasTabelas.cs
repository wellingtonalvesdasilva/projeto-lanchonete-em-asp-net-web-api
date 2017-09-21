namespace Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AjustesNasTabelas : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Ingredientes", newName: "Ingrediente");
            RenameTable(name: "dbo.Lanches", newName: "Lanche");
            RenameTable(name: "dbo.Porcaos", newName: "Porcao");
            AlterColumn("dbo.Ingrediente", "Nome", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AlterColumn("dbo.Lanche", "Nome", c => c.String(nullable: false, maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lanche", "Nome", c => c.String(nullable: false));
            AlterColumn("dbo.Ingrediente", "Nome", c => c.String(nullable: false));
            RenameTable(name: "dbo.Porcao", newName: "Porcaos");
            RenameTable(name: "dbo.Lanche", newName: "Lanches");
            RenameTable(name: "dbo.Ingrediente", newName: "Ingredientes");
        }
    }
}
