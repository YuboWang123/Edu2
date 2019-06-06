namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class endDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FINCards", "EndDay", c => c.DateTime());
            AlterColumn("dbo.FINCards", "StatusDay", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FINCards", "StatusDay", c => c.DateTime());
            DropColumn("dbo.FINCards", "EndDay");
        }
    }
}
