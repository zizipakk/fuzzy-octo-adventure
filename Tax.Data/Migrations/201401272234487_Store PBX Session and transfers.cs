namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StorePBXSessionandtransfers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PBXSessions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CallerCallId = c.String(nullable: false, maxLength: 100),
                        CalleeCallId = c.String(nullable: false, maxLength: 100),
                        CallDirection = c.Int(nullable: false),
                        CallerId = c.String(nullable: false),
                        Destination = c.String(nullable: false),
                        DialedNumber = c.String(nullable: false),
                        RingDuration = c.Time(nullable: false, precision: 7),
                        SessionID = c.String(nullable: false),
                        Source = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        StateDuration = c.Time(nullable: false, precision: 7),
                        TalkDuration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.PBXSessions", new string[] { "CallerCallId", "CalleeCallId", "State" }, true, "IX_PBXSessions_CallIds");
            CreateIndex("dbo.PBXSessions", new string[] { "StartTime" }, false, "IX_PBXSessions_StartTime");

            CreateTable(
                "dbo.PBXTransfers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SessionOriginal_Id = c.Guid(nullable: false),
                        SessionTransferred_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PBXSessions", t => t.SessionOriginal_Id, cascadeDelete: false)
                .ForeignKey("dbo.PBXSessions", t => t.SessionTransferred_Id, cascadeDelete: false)
                .Index(t => t.SessionOriginal_Id)
                .Index(t => t.SessionTransferred_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PBXSessions", "IX_PBXSessions_CallIds");
            DropIndex("dbo.PBXSessions", "IX_PBXSessions_StartTime");

            DropForeignKey("dbo.PBXTransfers", "SessionTransferred_Id", "dbo.PBXSessions");
            DropForeignKey("dbo.PBXTransfers", "SessionOriginal_Id", "dbo.PBXSessions");
            DropIndex("dbo.PBXTransfers", new[] { "SessionTransferred_Id" });
            DropIndex("dbo.PBXTransfers", new[] { "SessionOriginal_Id" });
            DropTable("dbo.PBXTransfers");
            DropTable("dbo.PBXSessions");
        }
    }
}
