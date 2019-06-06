namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fileSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VcrFiles", "FileSize", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VcrFiles", "FileSize");
        }
    }
}
