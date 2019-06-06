namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vcrIsFree : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vcrs", "IsFree", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vcrs", "IsFree", c => c.Boolean());
        }
    }
}
