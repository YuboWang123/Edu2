namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vcrRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vcrs", "TrainerId", c => c.String(nullable: false));
            AlterColumn("dbo.Vcrs", "Maker", c => c.String(nullable: false));
            DropColumn("dbo.Vcrs", "IsBasic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vcrs", "IsBasic", c => c.Boolean());
            AlterColumn("dbo.Vcrs", "Maker", c => c.String());
            AlterColumn("dbo.Vcrs", "TrainerId", c => c.String());
        }
    }
}
