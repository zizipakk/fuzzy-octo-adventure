namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergemix : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "DataLog.DbTables",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            Name = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "DataLog.LogRecordChanges",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            RecordId = c.String(),
            //            Date = c.DateTime(nullable: false),
            //            UserId = c.String(maxLength: 128),
            //            DbTableId = c.Guid(nullable: false),
            //            ChangeType = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("DataLog.DbTables", t => t.DbTableId, cascadeDelete: true)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId)
            //    .Index(t => t.DbTableId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "DataLog.LogColumnChanges",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            LogRecordChangeId = c.Guid(nullable: false),
            //            Name = c.String(),
            //            OldValue = c.String(),
            //            NewValue = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("DataLog.LogRecordChanges", t => t.LogRecordChangeId, cascadeDelete: true)
            //    .Index(t => t.LogRecordChangeId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("DataLog.LogRecordChanges", "UserId", "dbo.AspNetUsers");
            //DropForeignKey("DataLog.LogColumnChanges", "LogRecordChangeId", "DataLog.LogRecordChanges");
            //DropForeignKey("DataLog.LogRecordChanges", "DbTableId", "DataLog.DbTables");
            //DropIndex("DataLog.LogRecordChanges", new[] { "UserId" });
            //DropIndex("DataLog.LogColumnChanges", new[] { "LogRecordChangeId" });
            //DropIndex("DataLog.LogRecordChanges", new[] { "DbTableId" });
            //DropTable("DataLog.LogColumnChanges");
            //DropTable("DataLog.LogRecordChanges");
            //DropTable("DataLog.DbTables");
        }
    }
}
