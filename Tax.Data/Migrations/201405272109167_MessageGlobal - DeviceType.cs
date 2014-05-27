namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageGlobalDeviceType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessagesLocalDeviceTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServiceResponse = c.String(),
                        DeviceType_Id = c.Guid(),
                        MessagesGlobal_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceType_Id)
                .ForeignKey("dbo.MessagesGlobals", t => t.MessagesGlobal_Id)
                .Index(t => t.DeviceType_Id)
                .Index(t => t.MessagesGlobal_Id);
            
            DropColumn("dbo.MessagesGlobals", "ServiceResponse");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MessagesGlobals", "ServiceResponse", c => c.String());
            DropForeignKey("dbo.MessagesLocalDeviceTypes", "MessagesGlobal_Id", "dbo.MessagesGlobals");
            DropForeignKey("dbo.MessagesLocalDeviceTypes", "DeviceType_Id", "dbo.DeviceTypes");
            DropIndex("dbo.MessagesLocalDeviceTypes", new[] { "MessagesGlobal_Id" });
            DropIndex("dbo.MessagesLocalDeviceTypes", new[] { "DeviceType_Id" });
            DropTable("dbo.MessagesLocalDeviceTypes");
        }
    }
}
