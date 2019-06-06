namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class duration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vcrs", "Duration", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vcrs", "Duration");
        }
    }
}
