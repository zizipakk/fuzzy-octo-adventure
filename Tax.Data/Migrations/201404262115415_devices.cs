namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devices : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.CategoriesGlobals", newName: "ExtrasGlobals");
            DropForeignKey("dbo.CategoriesGlobals", "ExtrasGlobal_Id", "dbo.ExtrasGlobals");
            DropIndex("dbo.CategoriesGlobals", new[] { "ExtrasGlobal_Id" });
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Token = c.String(),
                        DeviceType_Id = c.Guid(),
                        Language_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceType_Id)
                .ForeignKey("dbo.Languages", t => t.Language_Id)
                .Index(t => t.DeviceType_Id)
                .Index(t => t.Language_Id);
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ExtrasGlobals", "CategoriesGlobal_Id", c => c.Guid());
            CreateIndex("dbo.ExtrasGlobals", "CategoriesGlobal_Id");
            AddForeignKey("dbo.ExtrasGlobals", "CategoriesGlobal_Id", "dbo.CategoriesGlobals", "Id");
            DropColumn("dbo.CategoriesGlobals", "ExtrasGlobal_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CategoriesGlobals", "ExtrasGlobal_Id", c => c.Guid());
            DropForeignKey("dbo.ExtrasGlobals", "CategoriesGlobal_Id", "dbo.CategoriesGlobals");
            DropForeignKey("dbo.Devices", "Language_Id", "dbo.Languages");
            DropForeignKey("dbo.Devices", "DeviceType_Id", "dbo.DeviceTypes");
            DropIndex("dbo.ExtrasGlobals", new[] { "CategoriesGlobal_Id" });
            DropIndex("dbo.Devices", new[] { "Language_Id" });
            DropIndex("dbo.Devices", new[] { "DeviceType_Id" });
            DropColumn("dbo.ExtrasGlobals", "CategoriesGlobal_Id");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.Devices");
            CreateIndex("dbo.CategoriesGlobals", "ExtrasGlobal_Id");
            AddForeignKey("dbo.CategoriesGlobals", "ExtrasGlobal_Id", "dbo.ExtrasGlobals", "Id");
            //RenameTable(name: "dbo.ExtrasGlobals", newName: "CategoriesGlobals");
        }
    }
}
