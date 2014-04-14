namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bugpost : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BugPosts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        UserName = c.String(),
                        ProjectName = c.String(),
                        AreaName = c.String(),
                        Description = c.String(),
                        ExtraInformation = c.String(),
                        EmailAddress = c.String(),
                        IsForceNewBug = c.Boolean(nullable: false),
                        IsFriendlyResponse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BugPosts");
        }
    }
}
