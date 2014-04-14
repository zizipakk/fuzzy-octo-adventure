namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceUsage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceUsages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        IMEI = c.String(),
                        Serial = c.String(),
                        OS = c.String(),
                        OSVersion = c.String(),
                        ClientVersion = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceUsages", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DeviceUsages", new[] { "User_Id" });
            DropTable("dbo.DeviceUsages");
        }
    }
}
