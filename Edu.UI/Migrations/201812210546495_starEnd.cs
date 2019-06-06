namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class starEnd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCardConfigs", "Start", c => c.String());
            AddColumn("dbo.FINCardConfigs", "End", c => c.String());
            AddColumn("dbo.FINCards", "FINCardConfig_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.FINCards", "FINCardConfig_Id");
            AddForeignKey("dbo.FINCards", "FINCardConfig_Id", "dbo.FINCardConfigs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FINCards", "FINCardConfig_Id", "dbo.FINCardConfigs");
            DropIndex("dbo.FINCards", new[] { "FINCardConfig_Id" });
            DropColumn("dbo.FINCards", "FINCardConfig_Id");
            DropColumn("dbo.FINCardConfigs", "End");
            DropColumn("dbo.FINCardConfigs", "Start");
        }
    }
}
