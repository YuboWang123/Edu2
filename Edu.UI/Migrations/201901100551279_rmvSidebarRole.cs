namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rmvSidebarRole : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ConsoleSideBars", newName: "Modules");
            DropForeignKey("dbo.MenuModules", "ConsoleSideBarId", "dbo.ConsoleSideBars");
            DropForeignKey("dbo.SideBarRoles", "MenuModule_Id", "dbo.MenuModules");
            DropIndex("dbo.MenuModules", new[] { "ConsoleSideBarId" });
            DropIndex("dbo.SideBarRoles", new[] { "MenuModule_Id" });
            DropPrimaryKey("dbo.SideBarRoles");
            CreateTable(
                "dbo.ConsoleSideMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        ActionName = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsEnabled = c.Boolean(nullable: false),
                        OrderNo = c.Int(nullable: false),
                        Maker = c.String(),
                        SchoolId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId);
            
            AddColumn("dbo.SideBarRoles", "ConsoleSideMenuId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.SideBarRoles", new[] { "ApplicationRoleId", "ConsoleSideMenuId" });
            CreateIndex("dbo.SideBarRoles", "ConsoleSideMenuId");
            AddForeignKey("dbo.SideBarRoles", "ConsoleSideMenuId", "dbo.ConsoleSideMenus", "Id", cascadeDelete: true);
            DropColumn("dbo.SideBarRoles", "ConsoleSideBarContentId");
            DropColumn("dbo.SideBarRoles", "MenuModule_Id");
            DropTable("dbo.MenuModules");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MenuModules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsoleSideBarId = c.Int(nullable: false),
                        ActionName = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsEnabled = c.Boolean(nullable: false),
                        OrderNo = c.Int(nullable: false),
                        Maker = c.String(),
                        SchoolId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SideBarRoles", "MenuModule_Id", c => c.Int());
            AddColumn("dbo.SideBarRoles", "ConsoleSideBarContentId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SideBarRoles", "ConsoleSideMenuId", "dbo.ConsoleSideMenus");
            DropForeignKey("dbo.ConsoleSideMenus", "ModuleId", "dbo.Modules");
            DropIndex("dbo.SideBarRoles", new[] { "ConsoleSideMenuId" });
            DropIndex("dbo.ConsoleSideMenus", new[] { "ModuleId" });
            DropPrimaryKey("dbo.SideBarRoles");
            DropColumn("dbo.SideBarRoles", "ConsoleSideMenuId");
            DropTable("dbo.ConsoleSideMenus");
            AddPrimaryKey("dbo.SideBarRoles", new[] { "ApplicationRoleId", "ConsoleSideBarContentId" });
            CreateIndex("dbo.SideBarRoles", "MenuModule_Id");
            CreateIndex("dbo.MenuModules", "ConsoleSideBarId");
            AddForeignKey("dbo.SideBarRoles", "MenuModule_Id", "dbo.MenuModules", "Id");
            AddForeignKey("dbo.MenuModules", "ConsoleSideBarId", "dbo.ConsoleSideBars", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Modules", newName: "ConsoleSideBars");
        }
    }
}
