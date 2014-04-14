namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comment_and_Survey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                        Client_Id = c.String(maxLength: 128),
                        Interperter_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Client_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Interperter_Id)
                .Index(t => t.Client_Id)
                .Index(t => t.Interperter_Id);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        SurveyType = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SurveyAnswers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                        Survey_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Surveys", t => t.Survey_Id)
                .Index(t => t.Survey_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Surveys", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyAnswers", "Survey_Id", "dbo.Surveys");
            DropForeignKey("dbo.Comments", "Interperter_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Client_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Surveys", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SurveyAnswers", new[] { "Survey_Id" });
            DropIndex("dbo.Comments", new[] { "Interperter_Id" });
            DropIndex("dbo.Comments", new[] { "Client_Id" });
            DropTable("dbo.SurveyAnswers");
            DropTable("dbo.Surveys");
            DropTable("dbo.Comments");
        }
    }
}
