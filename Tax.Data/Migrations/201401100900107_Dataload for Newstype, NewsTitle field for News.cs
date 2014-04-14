namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataloadforNewstypeNewsTitlefieldforNews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "NewsTitle", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.News", "NewsTitle");
        }
    }
}
