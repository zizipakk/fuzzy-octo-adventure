namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class engagetagcontactrelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagsGlobals", "ContactsGlobal_Id", "dbo.ContactsGlobals");
            DropIndex("dbo.TagsGlobals", new[] { "ContactsGlobal_Id" });
            DropColumn("dbo.TagsGlobals", "ContactsGlobal_Id");

            CreateTable(
                "dbo.TagsGlobalContactsGlobals",
                c => new
                {
                    TagsGlobal_Id = c.Guid(nullable: false),
                    ContactsGlobal_Id = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.TagsGlobal_Id, t.ContactsGlobal_Id })
                .ForeignKey("dbo.TagsGlobals", t => t.TagsGlobal_Id, cascadeDelete: true)
                .ForeignKey("dbo.ContactsGlobals", t => t.ContactsGlobal_Id, cascadeDelete: true)
                .Index(t => t.TagsGlobal_Id)
                .Index(t => t.ContactsGlobal_Id);
        }
        
        public override void Down()
        {
            AddColumn("dbo.TagsGlobals", "ContactsGlobal_Id", c => c.Guid());
            DropForeignKey("dbo.TagsGlobalContactsGlobals", "ContactsGlobal_Id", "dbo.ContactsGlobals");
            DropForeignKey("dbo.TagsGlobalContactsGlobals", "TagsGlobal_Id", "dbo.TagsGlobals");
            DropIndex("dbo.TagsGlobalContactsGlobals", new[] { "ContactsGlobal_Id" });
            DropIndex("dbo.TagsGlobalContactsGlobals", new[] { "TagsGlobal_Id" });
            CreateIndex("dbo.TagsGlobals", "ContactsGlobal_Id");
            AddForeignKey("dbo.TagsGlobals", "ContactsGlobal_Id", "dbo.ContactsGlobals", "Id");
            DropTable("dbo.TagsGlobalContactsGlobals");
        }
    }
}
