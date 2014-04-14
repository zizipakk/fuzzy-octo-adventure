namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trytomapfiletable : DbMigration
    {
        public override void Up()
        {
            //TODO: Attributummal mappelek
            //CreateTable(
            //    "dbo.Files",
            //    c => new
            //        {
            //            stream_id = c.Guid(nullable: false),
            //            file_stream = c.String(),
            //            name = c.String(),
            //        })
            //    .PrimaryKey(t => t.stream_id);
            
            DropColumn("dbo.SinoszUsers", "UserPictId");
            DropColumn("dbo.AttachedFiles", "AttachedFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AttachedFiles", "AttachedFileName", c => c.String());
            AddColumn("dbo.SinoszUsers", "UserPictId", c => c.Guid(nullable: false));
            //DropTable("dbo.Files");
        }
    }
}
