namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestMemo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Base_DataBind", "Memo", c => c.String());
            AddColumn("dbo.VcrFiles", "Memo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VcrFiles", "Memo");
            DropColumn("dbo.Base_DataBind", "Memo");
        }
    }
}
