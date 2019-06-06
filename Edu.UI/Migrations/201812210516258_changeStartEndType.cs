namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeStartEndType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FINCardConfigs", "Start", c => c.String(nullable: false));
            AlterColumn("dbo.FINCardConfigs", "End", c => c.String(nullable: false));
            AlterColumn("dbo.FINCards", "ActivatedDay", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FINCards", "ActivatedDay", c => c.DateTime());
            AlterColumn("dbo.FINCardConfigs", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FINCardConfigs", "Start", c => c.DateTime(nullable: false));
        }
    }
}
