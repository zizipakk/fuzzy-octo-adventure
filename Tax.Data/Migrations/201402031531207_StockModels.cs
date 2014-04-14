namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceLogDate = c.DateTime(nullable: false),
                        Remark = c.String(),
                        Area_Id = c.Guid(),
                        Devices_Id = c.Guid(),
                        KontaktUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.Area_Id)
                .ForeignKey("dbo.Devices", t => t.Devices_Id)
                .ForeignKey("dbo.KontaktUsers", t => t.KontaktUser_Id)
                .Index(t => t.Area_Id)
                .Index(t => t.Devices_Id)
                .Index(t => t.KontaktUser_Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceId = c.String(),
                        AccountingId = c.String(nullable: false),
                        DeviceName = c.String(),
                        DeviceStatus_Id = c.Guid(),
                        DeviceTypes_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceStatus", t => t.DeviceStatus_Id)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypes_Id)
                .Index(t => t.DeviceStatus_Id)
                .Index(t => t.DeviceTypes_Id);
            
            CreateTable(
                "dbo.DeviceStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceStatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeviceTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceLogs", "KontaktUser_Id", "dbo.KontaktUsers");
            DropForeignKey("dbo.Devices", "DeviceTypes_Id", "dbo.DeviceTypes");
            DropForeignKey("dbo.Devices", "DeviceStatus_Id", "dbo.DeviceStatus");
            DropForeignKey("dbo.DeviceLogs", "Devices_Id", "dbo.Devices");
            DropForeignKey("dbo.DeviceLogs", "Area_Id", "dbo.Areas");
            DropIndex("dbo.DeviceLogs", new[] { "KontaktUser_Id" });
            DropIndex("dbo.Devices", new[] { "DeviceTypes_Id" });
            DropIndex("dbo.Devices", new[] { "DeviceStatus_Id" });
            DropIndex("dbo.DeviceLogs", new[] { "Devices_Id" });
            DropIndex("dbo.DeviceLogs", new[] { "Area_Id" });
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.DeviceStatus");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceLogs");
        }
    }
}
