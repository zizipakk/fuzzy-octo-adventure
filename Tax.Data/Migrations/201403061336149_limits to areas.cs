namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limitstoareas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcodes", "Area_Id", c => c.Guid());
            AddColumn("dbo.Areas", "PhoneNumberLimit", c => c.Int(nullable: false));
            AddColumn("dbo.Areas", "DeviceNumberLimit", c => c.Int(nullable: false));
            CreateIndex("dbo.Postcodes", "Area_Id");
            AddForeignKey("dbo.Postcodes", "Area_Id", "dbo.Areas", "Id");
            DropColumn("dbo.Organizations", "PhoneNumberLimit");
            DropColumn("dbo.Organizations", "DeviceNumberLimit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organizations", "DeviceNumberLimit", c => c.Int(nullable: false));
            AddColumn("dbo.Organizations", "PhoneNumberLimit", c => c.Int(nullable: false));
            DropForeignKey("dbo.Postcodes", "Area_Id", "dbo.Areas");
            DropIndex("dbo.Postcodes", new[] { "Area_Id" });
            DropColumn("dbo.Areas", "DeviceNumberLimit");
            DropColumn("dbo.Areas", "PhoneNumberLimit");
            DropColumn("dbo.Postcodes", "Area_Id");
        }
    }
}
