namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublishingDatetoNews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "PublishingDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "PublishingDate");
        }
    }
}
