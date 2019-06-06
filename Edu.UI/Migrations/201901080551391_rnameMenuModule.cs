namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rnameMenuModule : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ConsoleSideBarContents", newName: "MenuModules");
            DropForeignKey("dbo.SideBarRoles", "ConsoleSideBarContentId", "dbo.ConsoleSideBarContents");
            DropIndex("dbo.SideBarRoles", new[] { "ConsoleSideBarContentId" });
            AddColumn("dbo.SideBarRoles", "MenuModule_Id", c => c.Int());
            CreateIndex("dbo.SideBarRoles", "MenuModule_Id");
            AddForeignKey("dbo.SideBarRoles", "MenuModule_Id", "dbo.MenuModules", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SideBarRoles", "MenuModule_Id", "dbo.MenuModules");
            DropIndex("dbo.SideBarRoles", new[] { "MenuModule_Id" });
            DropColumn("dbo.SideBarRoles", "MenuModule_Id");
            CreateIndex("dbo.SideBarRoles", "ConsoleSideBarContentId");
            AddForeignKey("dbo.SideBarRoles", "ConsoleSideBarContentId", "dbo.ConsoleSideBarContents", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.MenuModules", newName: "ConsoleSideBarContents");
        }
    }
}
