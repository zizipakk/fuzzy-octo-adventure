namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergeetwas : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.PBXTransfers", "SessionOriginal_Id", "dbo.PBXSessions");
            //DropForeignKey("dbo.PBXTransfers", "SessionTransferred_Id", "dbo.PBXSessions");
            //DropIndex("dbo.PBXTransfers", new[] { "SessionOriginal_Id" });
            //DropIndex("dbo.PBXTransfers", new[] { "SessionTransferred_Id" });
            //AddColumn("dbo.PBXTransfers", "CallIdOriginal", c => c.String(nullable: false));
            //AddColumn("dbo.PBXTransfers", "CallIdTransferred", c => c.String(nullable: false));
            //DropColumn("dbo.PBXTransfers", "SessionOriginal_Id");
            //DropColumn("dbo.PBXTransfers", "SessionTransferred_Id");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.PBXTransfers", "SessionTransferred_Id", c => c.Guid(nullable: false));
            //AddColumn("dbo.PBXTransfers", "SessionOriginal_Id", c => c.Guid(nullable: false));
            //DropColumn("dbo.PBXTransfers", "CallIdTransferred");
            //DropColumn("dbo.PBXTransfers", "CallIdOriginal");
            //CreateIndex("dbo.PBXTransfers", "SessionTransferred_Id");
            //CreateIndex("dbo.PBXTransfers", "SessionOriginal_Id");
            //AddForeignKey("dbo.PBXTransfers", "SessionTransferred_Id", "dbo.PBXSessions", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.PBXTransfers", "SessionOriginal_Id", "dbo.PBXSessions", "Id", cascadeDelete: true);
        }
    }
}
