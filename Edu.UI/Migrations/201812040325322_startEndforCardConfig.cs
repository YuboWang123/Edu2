namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class startEndforCardConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCardConfigs", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.FINCardConfigs", "End", c => c.DateTime(nullable: false));
            DropColumn("dbo.FINCardConfigs", "StartDay");
            DropColumn("dbo.FINCardConfigs", "EndDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FINCardConfigs", "EndDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.FINCardConfigs", "StartDay", c => c.DateTime(nullable: false));
            DropColumn("dbo.FINCardConfigs", "End");
            DropColumn("dbo.FINCardConfigs", "Start");
        }
    }
}
