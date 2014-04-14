namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Organizationmissingproperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "Address", c => c.String());
            AddColumn("dbo.Organizations", "Postcode_Id", c => c.Guid());
            AddColumn("dbo.Organizations", "Organization_Id", c => c.Guid());
            CreateIndex("dbo.Organizations", "Postcode_Id");
            CreateIndex("dbo.Organizations", "Organization_Id");
            AddForeignKey("dbo.Organizations", "Postcode_Id", "dbo.Postcodes", "Id");
            AddForeignKey("dbo.Organizations", "Organization_Id", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "Postcode_Id", "dbo.Postcodes");
            DropIndex("dbo.Organizations", new[] { "Organization_Id" });
            DropIndex("dbo.Organizations", new[] { "Postcode_Id" });
            DropColumn("dbo.Organizations", "Organization_Id");
            DropColumn("dbo.Organizations", "Postcode_Id");
            DropColumn("dbo.Organizations", "Address");
        }
    }
}
