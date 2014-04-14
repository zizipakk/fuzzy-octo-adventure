namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anothertrytofile_type : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Files", "file_type");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Files", "file_type", c => c.String());
        }
    }
}
