namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactsorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactsGlobals", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactsGlobals", "Order");
        }
    }
}
