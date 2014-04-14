namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmenusFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubMenus", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.SubMenus", new[] { "Menu_Id" });
            CreateIndex("dbo.SubMenus", "Menu_Id");
            AddForeignKey("dbo.SubMenus", "Menu_Id", "dbo.Menus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubMenus", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.SubMenus", new[] { "Menu_Id" });
            CreateIndex("dbo.SubMenus", "Menu_Id");
            AddForeignKey("dbo.SubMenus", "Menu_Id", "dbo.Menus", "Id");
        }
    }
}
