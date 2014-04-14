namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prregistrationdata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreregistrationDatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsNeedForJob = c.Boolean(nullable: false),
                        Job = c.String(),
                        IsNeedForHealth = c.Boolean(nullable: false),
                        IsNeedForLife = c.Boolean(nullable: false),
                        SocialPosition_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SocialPositions", t => t.SocialPosition_Id)
                .Index(t => t.SocialPosition_Id);
            
            CreateTable(
                "dbo.SocialPositions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SocialPositionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.KontaktUsers", "PreregistrationData_Id", c => c.Guid());
            AddColumn("dbo.AspNetUsers", "Organization_Id", c => c.Guid());
            CreateIndex("dbo.KontaktUsers", "PreregistrationData_Id");
            CreateIndex("dbo.AspNetUsers", "Organization_Id");
            AddForeignKey("dbo.KontaktUsers", "PreregistrationData_Id", "dbo.PreregistrationDatas", "Id");
            AddForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.KontaktUsers", "PreregistrationData_Id", "dbo.PreregistrationDatas");
            DropForeignKey("dbo.PreregistrationDatas", "SocialPosition_Id", "dbo.SocialPositions");
            DropIndex("dbo.AspNetUsers", new[] { "Organization_Id" });
            DropIndex("dbo.KontaktUsers", new[] { "PreregistrationData_Id" });
            DropIndex("dbo.PreregistrationDatas", new[] { "SocialPosition_Id" });
            DropColumn("dbo.AspNetUsers", "Organization_Id");
            DropColumn("dbo.KontaktUsers", "PreregistrationData_Id");
            DropTable("dbo.SocialPositions");
            DropTable("dbo.PreregistrationDatas");
        }
    }
}
