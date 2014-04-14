namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class file_typefinaly : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Files", "mock");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Files", "mock", c => c.String());
        }
    }
}
