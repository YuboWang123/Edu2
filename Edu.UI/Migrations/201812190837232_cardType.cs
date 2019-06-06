namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cardType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCardConfigs", "CardType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FINCardConfigs", "CardType");
        }
    }
}
