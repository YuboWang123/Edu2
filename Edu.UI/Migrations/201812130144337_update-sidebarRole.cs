namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatesidebarRole : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SideBarRoles", "ConsoleSideBarId", "dbo.ConsoleSideBars");
            DropIndex("dbo.SideBarRoles", new[] { "ConsoleSideBarId" });
            DropPrimaryKey("dbo.SideBarRoles");
            AddColumn("dbo.SideBarRoles", "ConsoleSideBarContentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SideBarRoles", new[] { "ApplicationRoleId", "ConsoleSideBarContentId" });
            CreateIndex("dbo.SideBarRoles", "ConsoleSideBarContentId");
            AddForeignKey("dbo.SideBarRoles", "ConsoleSideBarContentId", "dbo.ConsoleSideBarContents", "Id", cascadeDelete: true);
            DropColumn("dbo.SideBarRoles", "ConsoleSideBarId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SideBarRoles", "ConsoleSideBarId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SideBarRoles", "ConsoleSideBarContentId", "dbo.ConsoleSideBarContents");
            DropIndex("dbo.SideBarRoles", new[] { "ConsoleSideBarContentId" });
            DropPrimaryKey("dbo.SideBarRoles");
            DropColumn("dbo.SideBarRoles", "ConsoleSideBarContentId");
            AddPrimaryKey("dbo.SideBarRoles", new[] { "ApplicationRoleId", "ConsoleSideBarId" });
            CreateIndex("dbo.SideBarRoles", "ConsoleSideBarId");
            AddForeignKey("dbo.SideBarRoles", "ConsoleSideBarId", "dbo.ConsoleSideBars", "Id", cascadeDelete: true);
        }
    }
}
