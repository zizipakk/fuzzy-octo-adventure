namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contenttypeforfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "content_type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "content_type");
        }
    }
}
