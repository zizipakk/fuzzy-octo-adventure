namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filesneedthefile_type : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Files", "file_type", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Files", "file_type");
        }
    }
}
