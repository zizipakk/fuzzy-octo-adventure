namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationstoKontaktUserRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.AspNetUsers", new[] { "Organization_Id" });
            AddColumn("dbo.KontaktUserRoles", "Organization_Id", c => c.Guid());
            CreateIndex("dbo.KontaktUserRoles", "Organization_Id");
            AddForeignKey("dbo.KontaktUserRoles", "Organization_Id", "dbo.Organizations", "Id");
            DropColumn("dbo.AspNetUsers", "Organization_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Organization_Id", c => c.Guid());
            DropForeignKey("dbo.KontaktUserRoles", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.KontaktUserRoles", new[] { "Organization_Id" });
            DropColumn("dbo.KontaktUserRoles", "Organization_Id");
            CreateIndex("dbo.AspNetUsers", "Organization_Id");
            AddForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations", "Id");
        }
    }
}
