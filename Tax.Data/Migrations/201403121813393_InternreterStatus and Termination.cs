namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternreterStatusandTermination : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PBXInterpreterStatus",
                c => new
                    {
                        Extension = c.String(nullable: false, maxLength: 10),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Extension);
            
            CreateTable(
                "dbo.PBXInterpreterTerminateCallQueues",
                c => new
                    {
                        Extension = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Extension);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PBXInterpreterTerminateCallQueues");
            DropTable("dbo.PBXInterpreterStatus");
        }
    }
}
