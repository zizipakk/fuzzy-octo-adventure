namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OzekiSQLAPI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ozpbxcalls",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        Source = c.String(nullable: false, maxLength: 50),
                        CallerID = c.String(nullable: false, maxLength: 50),
                        Destination = c.String(nullable: false, maxLength: 50),
                        Duration = c.String(nullable: false, maxLength: 20),
                        CallState = c.String(nullable: false, maxLength: 50),
                        RecordURL = c.String(maxLength: 100),
                        Dialed = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ozpbxlog",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Thread = c.Int(nullable: false),
                        Level = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false, maxLength: 10),
                        Logger = c.String(nullable: false, maxLength: 100),
                        Message = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ozpbxlog");
            DropTable("dbo.ozpbxcalls");
        }
    }
}
