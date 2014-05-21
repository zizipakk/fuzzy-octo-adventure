namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoogleAppleresponsetexttoMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessagesGlobals", "ServiceResponse", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessagesGlobals", "ServiceResponse");
        }
    }
}
