namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nameglobaltostatus : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NewsGlobals", "Headline_picture_Id", "dbo.AttachedFiles");
            DropForeignKey("dbo.NewsGlobals", "Thumbnail_Id", "dbo.AttachedFiles");
            DropForeignKey("dbo.ContactsGlobals", "Photo_Id", "dbo.AttachedFiles");
            DropIndex("dbo.NewsGlobals", new[] { "Headline_picture_Id" });
            DropIndex("dbo.NewsGlobals", new[] { "Thumbnail_Id" });
            DropIndex("dbo.ContactsGlobals", new[] { "Photo_Id" });
            RenameColumn(table: "dbo.NewsGlobals", name: "Headline_picture_Id", newName: "Headline_picture_stream_id");
            RenameColumn(table: "dbo.NewsGlobals", name: "Thumbnail_Id", newName: "Thumbnail_stream_id");
            RenameColumn(table: "dbo.ContactsGlobals", name: "Photo_Id", newName: "Photo_stream_id");
            AddColumn("dbo.NewsStatusesGlobals", "NameGlobal", c => c.String());
            CreateIndex("dbo.NewsGlobals", "Headline_picture_stream_id");
            CreateIndex("dbo.NewsGlobals", "Thumbnail_stream_id");
            CreateIndex("dbo.ContactsGlobals", "Photo_stream_id");
            AddForeignKey("dbo.NewsGlobals", "Headline_picture_stream_id", "dbo.Files", "stream_id");
            AddForeignKey("dbo.NewsGlobals", "Thumbnail_stream_id", "dbo.Files", "stream_id");
            AddForeignKey("dbo.ContactsGlobals", "Photo_stream_id", "dbo.Files", "stream_id");
            DropTable("dbo.AttachedFiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AttachedFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ContactsGlobals", "Photo_stream_id", "dbo.Files");
            DropForeignKey("dbo.NewsGlobals", "Thumbnail_stream_id", "dbo.Files");
            DropForeignKey("dbo.NewsGlobals", "Headline_picture_stream_id", "dbo.Files");
            DropIndex("dbo.ContactsGlobals", new[] { "Photo_stream_id" });
            DropIndex("dbo.NewsGlobals", new[] { "Thumbnail_stream_id" });
            DropIndex("dbo.NewsGlobals", new[] { "Headline_picture_stream_id" });
            DropColumn("dbo.NewsStatusesGlobals", "NameGlobal");
            RenameColumn(table: "dbo.ContactsGlobals", name: "Photo_stream_id", newName: "Photo_Id");
            RenameColumn(table: "dbo.NewsGlobals", name: "Thumbnail_stream_id", newName: "Thumbnail_Id");
            RenameColumn(table: "dbo.NewsGlobals", name: "Headline_picture_stream_id", newName: "Headline_picture_Id");
            CreateIndex("dbo.ContactsGlobals", "Photo_Id");
            CreateIndex("dbo.NewsGlobals", "Thumbnail_Id");
            CreateIndex("dbo.NewsGlobals", "Headline_picture_Id");
            AddForeignKey("dbo.ContactsGlobals", "Photo_Id", "dbo.AttachedFiles", "Id");
            AddForeignKey("dbo.NewsGlobals", "Thumbnail_Id", "dbo.AttachedFiles", "Id");
            AddForeignKey("dbo.NewsGlobals", "Headline_picture_Id", "dbo.AttachedFiles", "Id");
        }
    }
}
