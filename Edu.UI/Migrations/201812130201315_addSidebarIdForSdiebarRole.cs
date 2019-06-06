namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSidebarIdForSdiebarRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SideBarRoles", "SideBarId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SideBarRoles", "SideBarId");
        }
    }
}
