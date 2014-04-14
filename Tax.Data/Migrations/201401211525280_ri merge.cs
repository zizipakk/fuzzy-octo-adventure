namespace Kontakt.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rimerge : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ChatMessages",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            SenderUserName = c.String(),
            //            ReceiverUserName = c.String(),
            //            MessageText = c.String(),
            //            Timestamp = c.DateTime(nullable: false),
            //            SentTimestamp = c.DateTime(),
            //            DeliveredTimetamp = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.ChatMessages");
        }
    }
}
