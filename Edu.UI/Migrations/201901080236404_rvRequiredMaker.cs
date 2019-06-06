namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rvRequiredMaker : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vcrs", "TrainerId", c => c.String());
            AlterColumn("dbo.Vcrs", "Maker", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vcrs", "Maker", c => c.String(nullable: false));
            AlterColumn("dbo.Vcrs", "TrainerId", c => c.String(nullable: false));
        }
    }
}
