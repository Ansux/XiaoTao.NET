namespace xiaotao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.xt_store", "verify", c => c.Boolean(nullable: false));
            AlterColumn("dbo.xt_store", "states", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.xt_store", "states", c => c.Boolean());
            AlterColumn("dbo.xt_store", "verify", c => c.Boolean());
        }
    }
}
