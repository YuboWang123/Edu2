namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvSchoolId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TempUsers", "SchoolId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TempUsers", "SchoolId", c => c.String(maxLength: 200));
        }
    }
}
