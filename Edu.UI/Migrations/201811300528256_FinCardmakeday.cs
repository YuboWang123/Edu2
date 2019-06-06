namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinCardmakeday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCardConfigs", "MakeDay", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FINCardConfigs", "MakeDay");
        }
    }
}
