namespace xiaotao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _62 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.xt_store_log", "remark", c => c.String(maxLength: 200));
            AddColumn("dbo.xt_student_log", "remark", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.xt_student_log", "remark");
            DropColumn("dbo.xt_store_log", "remark");
        }
    }
}
