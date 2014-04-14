namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLimitTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LimitTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LimitName = c.String(),
                        MaxLimitPerYear = c.Single(nullable: false),
                        MinLimitPerQyear = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PBXExtensionDatas", "LimitType_Id", c => c.Guid());
            CreateIndex("dbo.PBXExtensionDatas", "LimitType_Id");
            AddForeignKey("dbo.PBXExtensionDatas", "LimitType_Id", "dbo.LimitTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PBXExtensionDatas", "LimitType_Id", "dbo.LimitTypes");
            DropIndex("dbo.PBXExtensionDatas", new[] { "LimitType_Id" });
            DropColumn("dbo.PBXExtensionDatas", "LimitType_Id");
            DropTable("dbo.LimitTypes");
        }
    }
}
