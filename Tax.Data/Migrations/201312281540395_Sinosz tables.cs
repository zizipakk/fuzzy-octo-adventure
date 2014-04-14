namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sinosztables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountingStatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccountingDocuments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DocumentDate = c.DateTime(nullable: false),
                        DocumnetNumber = c.String(),
                        Sum = c.Single(nullable: false),
                        AccountingStatus_Id = c.Guid(),
                        AccountingType_Id = c.Guid(),
                        SinoszUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountingStatus", t => t.AccountingStatus_Id)
                .ForeignKey("dbo.AccountingTypes", t => t.AccountingType_Id)
                .ForeignKey("dbo.SinoszUsers", t => t.SinoszUser_Id)
                .Index(t => t.AccountingStatus_Id)
                .Index(t => t.AccountingType_Id)
                .Index(t => t.SinoszUser_Id);
            
            CreateTable(
                "dbo.AccountingTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountingTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AddressTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AddressTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AddressText = c.String(),
                        AddressType_Id = c.Guid(),
                        SinoszUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressTypes", t => t.AddressType_Id)
                .ForeignKey("dbo.SinoszUsers", t => t.SinoszUser_Id)
                .Index(t => t.AddressType_Id)
                .Index(t => t.SinoszUser_Id);
            
            CreateTable(
                "dbo.AttachedFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AttachedFileName = c.String(),
                        SinoszUser_Id = c.Guid(),
                        FileType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SinoszUsers", t => t.SinoszUser_Id)
                .ForeignKey("dbo.FileTypes", t => t.FileType_Id)
                .Index(t => t.SinoszUser_Id)
                .Index(t => t.FileType_Id);
            
            CreateTable(
                "dbo.SinoszLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActionTime = c.DateTime(nullable: false),
                        ActionName = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        SinoszUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.SinoszUsers", t => t.SinoszUser_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.SinoszUser_Id);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Remark = c.String(),
                        AccountingDocument_Id = c.Guid(),
                        CardStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountingDocuments", t => t.AccountingDocument_Id)
                .ForeignKey("dbo.CardStatus", t => t.CardStatus_Id)
                .Index(t => t.AccountingDocument_Id)
                .Index(t => t.CardStatus_Id);
            
            CreateTable(
                "dbo.CardStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CardStatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EducationName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Genus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        GenusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HearingStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        HearingStatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InjuryTimes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InjuryTimeText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaritalStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MaritalStatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NationText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NewsText = c.String(),
                        NewsType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsTypes", t => t.NewsType_Id)
                .Index(t => t.NewsType_Id);
            
            CreateTable(
                "dbo.NewsTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NewsTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrganizationName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PensionTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PensionTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Relationships",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RelationshipName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SinoszUserStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StatusName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusToStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromStatus_Id = c.Guid(),
                        ToStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SinoszUserStatus", t => t.FromStatus_Id)
                .ForeignKey("dbo.SinoszUserStatus", t => t.ToStatus_Id)
                .Index(t => t.FromStatus_Id)
                .Index(t => t.ToStatus_Id);
            
            AddColumn("dbo.SinoszUsers", "SinoszUserName", c => c.String());
            AddColumn("dbo.SinoszUsers", "BirthPlace", c => c.String());
            AddColumn("dbo.SinoszUsers", "MothersName", c => c.String());
            AddColumn("dbo.SinoszUsers", "HomeAddress", c => c.String());
            AddColumn("dbo.SinoszUsers", "EnterDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SinoszUsers", "DecreeNumber", c => c.String());
            AddColumn("dbo.SinoszUsers", "BirthName", c => c.String());
            AddColumn("dbo.SinoszUsers", "Remark", c => c.String());
            AddColumn("dbo.SinoszUsers", "Qualification", c => c.String());
            AddColumn("dbo.SinoszUsers", "UserPictId", c => c.Guid(nullable: false));
            AddColumn("dbo.SinoszUsers", "isImplant", c => c.Boolean(nullable: false));
            AddColumn("dbo.SinoszUsers", "isHearingAid", c => c.Boolean(nullable: false));
            AddColumn("dbo.SinoszUsers", "Postcode_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "Education_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "Genus_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "HearingStatus_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "InjuryTime_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "MaritalStatus_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "Nation_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "Organization_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "PensionType_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "Position_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "Relationship_Id", c => c.Guid());
            AddColumn("dbo.SinoszUsers", "SinoszUserStatus_Id", c => c.Guid());
            CreateIndex("dbo.SinoszUsers", "Postcode_Id");
            CreateIndex("dbo.SinoszUsers", "Education_Id");
            CreateIndex("dbo.SinoszUsers", "Genus_Id");
            CreateIndex("dbo.SinoszUsers", "HearingStatus_Id");
            CreateIndex("dbo.SinoszUsers", "InjuryTime_Id");
            CreateIndex("dbo.SinoszUsers", "MaritalStatus_Id");
            CreateIndex("dbo.SinoszUsers", "Nation_Id");
            CreateIndex("dbo.SinoszUsers", "Organization_Id");
            CreateIndex("dbo.SinoszUsers", "PensionType_Id");
            CreateIndex("dbo.SinoszUsers", "Position_Id");
            CreateIndex("dbo.SinoszUsers", "Relationship_Id");
            CreateIndex("dbo.SinoszUsers", "SinoszUserStatus_Id");
            AddForeignKey("dbo.SinoszUsers", "Postcode_Id", "dbo.Postcodes", "Id");
            AddForeignKey("dbo.SinoszUsers", "Education_Id", "dbo.Educations", "Id");
            AddForeignKey("dbo.SinoszUsers", "Genus_Id", "dbo.Genus", "Id");
            AddForeignKey("dbo.SinoszUsers", "HearingStatus_Id", "dbo.HearingStatus", "Id");
            AddForeignKey("dbo.SinoszUsers", "InjuryTime_Id", "dbo.InjuryTimes", "Id");
            AddForeignKey("dbo.SinoszUsers", "MaritalStatus_Id", "dbo.MaritalStatus", "Id");
            AddForeignKey("dbo.SinoszUsers", "Nation_Id", "dbo.Nations", "Id");
            AddForeignKey("dbo.SinoszUsers", "Organization_Id", "dbo.Organizations", "Id");
            AddForeignKey("dbo.SinoszUsers", "PensionType_Id", "dbo.PensionTypes", "Id");
            AddForeignKey("dbo.SinoszUsers", "Position_Id", "dbo.Positions", "Id");
            AddForeignKey("dbo.SinoszUsers", "Relationship_Id", "dbo.Relationships", "Id");
            AddForeignKey("dbo.SinoszUsers", "SinoszUserStatus_Id", "dbo.SinoszUserStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StatusToStatus", "ToStatus_Id", "dbo.SinoszUserStatus");
            DropForeignKey("dbo.StatusToStatus", "FromStatus_Id", "dbo.SinoszUserStatus");
            DropForeignKey("dbo.SinoszUsers", "SinoszUserStatus_Id", "dbo.SinoszUserStatus");
            DropForeignKey("dbo.SinoszUsers", "Relationship_Id", "dbo.Relationships");
            DropForeignKey("dbo.SinoszUsers", "Position_Id", "dbo.Positions");
            DropForeignKey("dbo.SinoszUsers", "PensionType_Id", "dbo.PensionTypes");
            DropForeignKey("dbo.SinoszUsers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.News", "NewsType_Id", "dbo.NewsTypes");
            DropForeignKey("dbo.SinoszUsers", "Nation_Id", "dbo.Nations");
            DropForeignKey("dbo.SinoszUsers", "MaritalStatus_Id", "dbo.MaritalStatus");
            DropForeignKey("dbo.SinoszUsers", "InjuryTime_Id", "dbo.InjuryTimes");
            DropForeignKey("dbo.SinoszUsers", "HearingStatus_Id", "dbo.HearingStatus");
            DropForeignKey("dbo.SinoszUsers", "Genus_Id", "dbo.Genus");
            DropForeignKey("dbo.AttachedFiles", "FileType_Id", "dbo.FileTypes");
            DropForeignKey("dbo.SinoszUsers", "Education_Id", "dbo.Educations");
            DropForeignKey("dbo.Cards", "CardStatus_Id", "dbo.CardStatus");
            DropForeignKey("dbo.Cards", "AccountingDocument_Id", "dbo.AccountingDocuments");
            DropForeignKey("dbo.SinoszUsers", "Postcode_Id", "dbo.Postcodes");
            DropForeignKey("dbo.SinoszLogs", "SinoszUser_Id", "dbo.SinoszUsers");
            DropForeignKey("dbo.SinoszLogs", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AttachedFiles", "SinoszUser_Id", "dbo.SinoszUsers");
            DropForeignKey("dbo.Addresses", "SinoszUser_Id", "dbo.SinoszUsers");
            DropForeignKey("dbo.AccountingDocuments", "SinoszUser_Id", "dbo.SinoszUsers");
            DropForeignKey("dbo.Addresses", "AddressType_Id", "dbo.AddressTypes");
            DropForeignKey("dbo.AccountingDocuments", "AccountingType_Id", "dbo.AccountingTypes");
            DropForeignKey("dbo.AccountingDocuments", "AccountingStatus_Id", "dbo.AccountingStatus");
            DropIndex("dbo.StatusToStatus", new[] { "ToStatus_Id" });
            DropIndex("dbo.StatusToStatus", new[] { "FromStatus_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "SinoszUserStatus_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Relationship_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Position_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "PensionType_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Organization_Id" });
            DropIndex("dbo.News", new[] { "NewsType_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Nation_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "MaritalStatus_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "InjuryTime_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "HearingStatus_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Genus_Id" });
            DropIndex("dbo.AttachedFiles", new[] { "FileType_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Education_Id" });
            DropIndex("dbo.Cards", new[] { "CardStatus_Id" });
            DropIndex("dbo.Cards", new[] { "AccountingDocument_Id" });
            DropIndex("dbo.SinoszUsers", new[] { "Postcode_Id" });
            DropIndex("dbo.SinoszLogs", new[] { "SinoszUser_Id" });
            DropIndex("dbo.SinoszLogs", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AttachedFiles", new[] { "SinoszUser_Id" });
            DropIndex("dbo.Addresses", new[] { "SinoszUser_Id" });
            DropIndex("dbo.AccountingDocuments", new[] { "SinoszUser_Id" });
            DropIndex("dbo.Addresses", new[] { "AddressType_Id" });
            DropIndex("dbo.AccountingDocuments", new[] { "AccountingType_Id" });
            DropIndex("dbo.AccountingDocuments", new[] { "AccountingStatus_Id" });
            DropColumn("dbo.SinoszUsers", "SinoszUserStatus_Id");
            DropColumn("dbo.SinoszUsers", "Relationship_Id");
            DropColumn("dbo.SinoszUsers", "Position_Id");
            DropColumn("dbo.SinoszUsers", "PensionType_Id");
            DropColumn("dbo.SinoszUsers", "Organization_Id");
            DropColumn("dbo.SinoszUsers", "Nation_Id");
            DropColumn("dbo.SinoszUsers", "MaritalStatus_Id");
            DropColumn("dbo.SinoszUsers", "InjuryTime_Id");
            DropColumn("dbo.SinoszUsers", "HearingStatus_Id");
            DropColumn("dbo.SinoszUsers", "Genus_Id");
            DropColumn("dbo.SinoszUsers", "Education_Id");
            DropColumn("dbo.SinoszUsers", "Postcode_Id");
            DropColumn("dbo.SinoszUsers", "isHearingAid");
            DropColumn("dbo.SinoszUsers", "isImplant");
            DropColumn("dbo.SinoszUsers", "UserPictId");
            DropColumn("dbo.SinoszUsers", "Qualification");
            DropColumn("dbo.SinoszUsers", "Remark");
            DropColumn("dbo.SinoszUsers", "BirthName");
            DropColumn("dbo.SinoszUsers", "DecreeNumber");
            DropColumn("dbo.SinoszUsers", "EnterDate");
            DropColumn("dbo.SinoszUsers", "HomeAddress");
            DropColumn("dbo.SinoszUsers", "MothersName");
            DropColumn("dbo.SinoszUsers", "BirthPlace");
            DropColumn("dbo.SinoszUsers", "SinoszUserName");
            DropTable("dbo.StatusToStatus");
            DropTable("dbo.SinoszUserStatus");
            DropTable("dbo.Relationships");
            DropTable("dbo.Positions");
            DropTable("dbo.PensionTypes");
            DropTable("dbo.Organizations");
            DropTable("dbo.NewsTypes");
            DropTable("dbo.News");
            DropTable("dbo.Nations");
            DropTable("dbo.MaritalStatus");
            DropTable("dbo.InjuryTimes");
            DropTable("dbo.HearingStatus");
            DropTable("dbo.Genus");
            DropTable("dbo.FileTypes");
            DropTable("dbo.Educations");
            DropTable("dbo.CardStatus");
            DropTable("dbo.Cards");
            DropTable("dbo.SinoszLogs");
            DropTable("dbo.AttachedFiles");
            DropTable("dbo.Addresses");
            DropTable("dbo.AddressTypes");
            DropTable("dbo.AccountingTypes");
            DropTable("dbo.AccountingDocuments");
            DropTable("dbo.AccountingStatus");
        }
    }
}
