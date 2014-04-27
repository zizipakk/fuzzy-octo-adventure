namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class indexestodevices : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Devices", "Token", c => c.String(maxLength: 1000));
            CreateIndex("dbo.Devices", "Token", false, "IX_Token", false);
            AlterColumn("dbo.DeviceTypes", "Name", c => c.String(maxLength: 100));
            CreateIndex("dbo.DeviceTypes", "Name", false, "IX_Name", false);
        }
        
        public override void Down()
        {
            DropIndex("dbo.DeviceTypes", "IX_Name");
            AlterColumn("dbo.DeviceTypes", "Name", c => c.String());
            DropIndex("dbo.Devices", "IX_Token");
            AlterColumn("dbo.Devices", "Token", c => c.String());
        }
    }
}
