namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coverpic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiveHostShows", "CoverPic", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LiveHostShows", "CoverPic");
        }
    }
}
