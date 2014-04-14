namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Organizationchangehimselfsconstraintstypewhilecodefirstmakeonewaynavproponicollectionprop : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Organizations", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.Organizations", new[] { "Organization_Id" });
            RenameColumn(table: "dbo.Organizations", name: "Organization_Id", newName: "UpperOrganization_Id");
            CreateIndex("dbo.Organizations", "UpperOrganization_Id");
            AddForeignKey("dbo.Organizations", "UpperOrganization_Id", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "UpperOrganization_Id", "dbo.Organizations");
            DropIndex("dbo.Organizations", new[] { "UpperOrganization_Id" });
            RenameColumn(table: "dbo.Organizations", name: "UpperOrganization_Id", newName: "Organization_Id");
            CreateIndex("dbo.Organizations", "Organization_Id");
            AddForeignKey("dbo.Organizations", "Organization_Id", "dbo.Organizations", "Id");
        }
    }
}
