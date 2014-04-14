namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class limitstoorganizations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "PhoneNumberLimit", c => c.Int(nullable: false));
            AddColumn("dbo.Organizations", "DeviceNumberLimit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "DeviceNumberLimit");
            DropColumn("dbo.Organizations", "PhoneNumberLimit");
        }
    }
}
