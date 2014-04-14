namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfileidtoattachedfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttachedFiles", "FileId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachedFiles", "FileId");
        }
    }
}
