namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isElectedinKontaktUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KontaktUsers", "isElected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KontaktUsers", "isElected");
        }
    }
}
