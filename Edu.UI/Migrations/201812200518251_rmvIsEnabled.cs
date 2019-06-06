namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvIsEnabled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCardConfigs", "StatusDay", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FINCards", "StatusDay", c => c.DateTime());
            DropColumn("dbo.FINCardConfigs", "IsEnabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FINCardConfigs", "IsEnabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.FINCards", "StatusDay", c => c.DateTime(nullable: false));
            DropColumn("dbo.FINCardConfigs", "StatusDay");
        }
    }
}
