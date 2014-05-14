namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class maptest : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TagsGlobalNewsGlobals", newName: "TagsGlobalNewsGlobal1");
            CreateTable(
                "dbo.TagsGlobalNewsGlobals",
                c => new
                    {
                        TagsGlobal_Id = c.Guid(nullable: false),
                        NewsGlobal_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagsGlobal_Id, t.NewsGlobal_Id })
                .ForeignKey("dbo.NewsGlobals", t => t.NewsGlobal_Id, cascadeDelete: true)
                .ForeignKey("dbo.TagsGlobals", t => t.TagsGlobal_Id, cascadeDelete: true)
                .Index(t => t.NewsGlobal_Id)
                .Index(t => t.TagsGlobal_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagsGlobalNewsGlobals", "TagsGlobal_Id", "dbo.TagsGlobals");
            DropForeignKey("dbo.TagsGlobalNewsGlobals", "NewsGlobal_Id", "dbo.NewsGlobals");
            DropIndex("dbo.TagsGlobalNewsGlobals", new[] { "TagsGlobal_Id" });
            DropIndex("dbo.TagsGlobalNewsGlobals", new[] { "NewsGlobal_Id" });
            DropTable("dbo.TagsGlobalNewsGlobals");
            RenameTable(name: "dbo.TagsGlobalNewsGlobal1", newName: "TagsGlobalNewsGlobals");
        }
    }
}
