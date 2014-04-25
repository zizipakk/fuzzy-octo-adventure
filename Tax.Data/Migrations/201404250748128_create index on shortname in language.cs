namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createindexonshortnameinlanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "ShortName", c => c.String(maxLength: 2));
            CreateIndex("dbo.Languages", "ShortName", false, "IX_ShortName", false);
        }

        public override void Down()
        {
            DropIndex("dbo.Languages", "IX_ShortName");
            DropColumn("dbo.Languages", "ShortName");
        }
    }
}
