namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rvEnd : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FINCardConfigs", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FINCardConfigs", "End", c => c.String(nullable: false));
        }
    }
}
