namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountPeriods : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountPeriods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PeriodBegin = c.DateTime(nullable: false),
                        PeriodEnd = c.DateTime(nullable: false),
                        AccountPeriodStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountPeriodStatus", t => t.AccountPeriodStatus_Id)
                .Index(t => t.AccountPeriodStatus_Id);
            
            CreateTable(
                "dbo.AccountPeriodStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountPeriods", "AccountPeriodStatus_Id", "dbo.AccountPeriodStatus");
            DropIndex("dbo.AccountPeriods", new[] { "AccountPeriodStatus_Id" });
            DropTable("dbo.AccountPeriodStatus");
            DropTable("dbo.AccountPeriods");
        }
    }
}
