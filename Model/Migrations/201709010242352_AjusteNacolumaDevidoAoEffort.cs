namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteNacolumaDevidoAoEffort : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ingrediente", "Nome", c => c.String(nullable: false));
            AlterColumn("dbo.Lanche", "Nome", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lanche", "Nome", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AlterColumn("dbo.Ingrediente", "Nome", c => c.String(nullable: false, maxLength: 60, unicode: false));
        }
    }
}
