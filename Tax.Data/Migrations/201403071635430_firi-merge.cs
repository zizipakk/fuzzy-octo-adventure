namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firimerge : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Postcodes", "Area_Id", c => c.Gui                        d());
            //AddColumn("dbo.Areas", "PhoneNumberLimit", c => c.Int(nullable: false));
            //AddColumn("dbo.Areas", "DeviceNumberLimit", c => c.Int(nullable: false));
            //CreateIndex("dbo.Postcodes", "Area_Id");
            //AddForeignKey("dbo.Postcodes", "Area_Id", "dbo.Areas", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.Postcodes", "Area_Id", "dbo.Areas");
            //DropIndex("dbo.Postcodes", new[] { "Area_Id" });
            //DropColumn("dbo.Areas", "DeviceNumberLimit");
            //DropColumn("dbo.Areas", "PhoneNumberLimit");
            //DropColumn("dbo.Postcodes", "Area_Id");
        }
    }
}
