namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesapplicationusersinoszuserrel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SinoszUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SinoszUsers", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.AspNetUsers", "SinoszUser_Id", c => c.Guid());
            CreateIndex("dbo.AspNetUsers", "SinoszUser_Id");
            AddForeignKey("dbo.AspNetUsers", "SinoszUser_Id", "dbo.SinoszUsers", "Id");
            DropColumn("dbo.SinoszUsers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SinoszUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "SinoszUser_Id", "dbo.SinoszUsers");
            DropIndex("dbo.AspNetUsers", new[] { "SinoszUser_Id" });
            DropColumn("dbo.AspNetUsers", "SinoszUser_Id");
            CreateIndex("dbo.SinoszUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.SinoszUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
