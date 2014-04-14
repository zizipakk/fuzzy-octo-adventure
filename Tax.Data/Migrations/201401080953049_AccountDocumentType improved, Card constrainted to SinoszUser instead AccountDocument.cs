namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountDocumentTypeimprovedCardconstraintedtoSinoszUserinsteadAccountDocument : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cards", "AccountingDocument_Id", "dbo.AccountingDocuments");
            DropIndex("dbo.Cards", new[] { "AccountingDocument_Id" });
            AddColumn("dbo.AccountingTypes", "isFixsum", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingTypes", "Presum", c => c.Single(nullable: false));
            AddColumn("dbo.AccountingTypes", "isMembershipCost", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingTypes", "isCardCost", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingTypes", "isEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingTypes", "Relationship_Id", c => c.Guid());
            AddColumn("dbo.Cards", "SinoszUser_Id", c => c.Guid());
            CreateIndex("dbo.AccountingTypes", "Relationship_Id");
            CreateIndex("dbo.Cards", "SinoszUser_Id");
            AddForeignKey("dbo.AccountingTypes", "Relationship_Id", "dbo.Relationships", "Id");
            AddForeignKey("dbo.Cards", "SinoszUser_Id", "dbo.SinoszUsers", "Id");
            DropColumn("dbo.Cards", "AccountingDocument_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cards", "AccountingDocument_Id", c => c.Guid());
            DropForeignKey("dbo.Cards", "SinoszUser_Id", "dbo.SinoszUsers");
            DropForeignKey("dbo.AccountingTypes", "Relationship_Id", "dbo.Relationships");
            DropIndex("dbo.Cards", new[] { "SinoszUser_Id" });
            DropIndex("dbo.AccountingTypes", new[] { "Relationship_Id" });
            DropColumn("dbo.Cards", "SinoszUser_Id");
            DropColumn("dbo.AccountingTypes", "Relationship_Id");
            DropColumn("dbo.AccountingTypes", "isEnabled");
            DropColumn("dbo.AccountingTypes", "isCardCost");
            DropColumn("dbo.AccountingTypes", "isMembershipCost");
            DropColumn("dbo.AccountingTypes", "Presum");
            DropColumn("dbo.AccountingTypes", "isFixsum");
            CreateIndex("dbo.Cards", "AccountingDocument_Id");
            AddForeignKey("dbo.Cards", "AccountingDocument_Id", "dbo.AccountingDocuments", "Id");
        }
    }
}
