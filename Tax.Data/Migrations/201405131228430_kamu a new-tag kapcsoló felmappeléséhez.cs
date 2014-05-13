namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kamuanewtagkapcsolófelmappeléséhez : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.TagsGlobalNewsGlobals", newName: "TagsGlobalNewsGlobal1");
            //DropForeignKey("dbo.ApplicationRoleSubMenus", "ApplicationRole_Id", "dbo.AspNetRoles");
            //DropForeignKey("dbo.ApplicationRoleSubMenus", "SubMenu_Id", "dbo.SubMenus");
            //DropIndex("dbo.ApplicationRoleSubMenus", new[] { "ApplicationRole_Id" });
            //DropIndex("dbo.ApplicationRoleSubMenus", new[] { "SubMenu_Id" });
            //CreateTable(
            //    "dbo.TagsGlobalNewsGlobals",
            //    c => new
            //        {
            //            TagsGlobal_Id = c.Guid(nullable: false),
            //            NewsGlobal_Id = c.Guid(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.TagsGlobal_Id, t.NewsGlobal_Id });
            
            //AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
            //DropColumn("dbo.AspNetRoles", "Discriminator");
            //DropTable("dbo.ApplicationRoleSubMenus");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.ApplicationRoleSubMenus",
            //    c => new
            //        {
            //            ApplicationRole_Id = c.String(nullable: false, maxLength: 128),
            //            SubMenu_Id = c.Guid(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.ApplicationRole_Id, t.SubMenu_Id });
            
            //AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            //AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            //DropTable("dbo.TagsGlobalNewsGlobals");
            //CreateIndex("dbo.ApplicationRoleSubMenus", "SubMenu_Id");
            //CreateIndex("dbo.ApplicationRoleSubMenus", "ApplicationRole_Id");
            //AddForeignKey("dbo.ApplicationRoleSubMenus", "SubMenu_Id", "dbo.SubMenus", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.ApplicationRoleSubMenus", "ApplicationRole_Id", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            //RenameTable(name: "dbo.TagsGlobalNewsGlobal1", newName: "TagsGlobalNewsGlobals");
        }
    }
}
