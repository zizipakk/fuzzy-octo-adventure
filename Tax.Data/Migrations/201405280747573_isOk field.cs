namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isOkfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessagesLocalDeviceTypes", "isOK", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessagesLocalDeviceTypes", "isOK");
        }
    }
}
