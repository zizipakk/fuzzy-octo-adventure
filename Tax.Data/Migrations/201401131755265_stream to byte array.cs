namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class streamtobytearray : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Files", "file_stream", c => c.Binary());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Files", "file_stream", c => c.String());
        }
    }
}
