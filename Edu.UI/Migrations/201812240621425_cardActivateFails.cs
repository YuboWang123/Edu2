namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cardActivateFails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCards", "FailTimes", c => c.Int(nullable: false));
            AddColumn("dbo.FINCards", "LockedEndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FINCards", "LockedEndTime");
            DropColumn("dbo.FINCards", "FailTimes");
        }
    }
}
