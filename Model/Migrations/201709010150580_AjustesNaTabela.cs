namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesNaTabela : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Porcao", name: "Ingrediente_Id", newName: "IngredienteId");
            RenameIndex(table: "dbo.Porcao", name: "IX_Ingrediente_Id", newName: "IX_IngredienteId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Porcao", name: "IX_IngredienteId", newName: "IX_Ingrediente_Id");
            RenameColumn(table: "dbo.Porcao", name: "IngredienteId", newName: "Ingrediente_Id");
        }
    }
}
