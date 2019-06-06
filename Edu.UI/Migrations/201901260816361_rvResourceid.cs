namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rvResourceid : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vcrs", "ResourceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vcrs", "ResourceId", c => c.Int());
        }
    }
}
