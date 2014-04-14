namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CallQueue_Register : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PBXCallQueues",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CallerExtension = c.String(nullable: false, maxLength: 100),
                        CalleeExtension = c.String(nullable: false, maxLength: 100),
                        CallId = c.String(nullable: false, maxLength: 200),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        FinishReason = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PBXRegistrations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Extension = c.String(nullable: false, maxLength: 100),
                        Registered = c.Boolean(nullable: false),
                        RegiserTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PBXRegistrations");
            DropTable("dbo.PBXCallQueues");
        }
    }
}
