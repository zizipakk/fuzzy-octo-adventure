namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttachedFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoriesGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        ExtrasGlobal_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtrasGlobals", t => t.ExtrasGlobal_Id)
                .Index(t => t.ExtrasGlobal_Id);
            
            CreateTable(
                "dbo.CategoriesLocals",
                c => new
                    {
                        CategoriesGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoriesGlobalId, t.LanguageId })
                .ForeignKey("dbo.CategoriesGlobals", t => t.CategoriesGlobalId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.CategoriesGlobalId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactsGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PublishingDate = c.DateTime(),
                        First_name = c.String(),
                        Last_name = c.String(),
                        Linkedin = c.String(),
                        Phone = c.String(),
                        Mobile = c.String(),
                        Email = c.String(),
                        NewsStatus_Id = c.Guid(),
                        Photo_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsStatusesGlobals", t => t.NewsStatus_Id)
                .ForeignKey("dbo.AttachedFiles", t => t.Photo_Id)
                .Index(t => t.NewsStatus_Id)
                .Index(t => t.Photo_Id);
            
            CreateTable(
                "dbo.ContactsLocals",
                c => new
                    {
                        ContactsGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Department = c.String(),
                        Position = c.String(),
                        Profile = c.String(),
                    })
                .PrimaryKey(t => new { t.ContactsGlobalId, t.LanguageId })
                .ForeignKey("dbo.ContactsGlobals", t => t.ContactsGlobalId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ContactsGlobalId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.NewsStatusesGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PublishingDate = c.DateTime(),
                        Headline_picture_Id = c.Guid(),
                        NewsStatus_Id = c.Guid(),
                        Thumbnail_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachedFiles", t => t.Headline_picture_Id)
                .ForeignKey("dbo.NewsStatusesGlobals", t => t.NewsStatus_Id)
                .ForeignKey("dbo.AttachedFiles", t => t.Thumbnail_Id)
                .Index(t => t.Headline_picture_Id)
                .Index(t => t.NewsStatus_Id)
                .Index(t => t.Thumbnail_Id);
            
            CreateTable(
                "dbo.NewsLocals",
                c => new
                    {
                        NewsGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Subtitle = c.String(),
                        Body_text = c.String(),
                    })
                .PrimaryKey(t => new { t.NewsGlobalId, t.LanguageId })
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.NewsGlobals", t => t.NewsGlobalId, cascadeDelete: true)
                .Index(t => t.LanguageId)
                .Index(t => t.NewsGlobalId);
            
            CreateTable(
                "dbo.TagsGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContactsGlobal_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactsGlobals", t => t.ContactsGlobal_Id)
                .Index(t => t.ContactsGlobal_Id);
            
            CreateTable(
                "dbo.TagsLocals",
                c => new
                    {
                        TagsGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagsGlobalId, t.LanguageId })
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.TagsGlobals", t => t.TagsGlobalId, cascadeDelete: true)
                .Index(t => t.LanguageId)
                .Index(t => t.TagsGlobalId);
            
            CreateTable(
                "dbo.NewsStatusesLocals",
                c => new
                    {
                        NewsStatusGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewsStatusGlobalId, t.LanguageId })
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.NewsStatusesGlobals", t => t.NewsStatusGlobalId, cascadeDelete: true)
                .Index(t => t.LanguageId)
                .Index(t => t.NewsStatusGlobalId);
            
            CreateTable(
                "dbo.EventsGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(),
                        PublishingDate = c.DateTime(),
                        NewsStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsStatusesGlobals", t => t.NewsStatus_Id)
                .Index(t => t.NewsStatus_Id);
            
            CreateTable(
                "dbo.EventsLocals",
                c => new
                    {
                        EventsGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Body_text = c.String(),
                    })
                .PrimaryKey(t => new { t.EventsGlobalId, t.LanguageId })
                .ForeignKey("dbo.EventsGlobals", t => t.EventsGlobalId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.EventsGlobalId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.ExtrasGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PublishingDate = c.DateTime(),
                        Order = c.Int(nullable: false),
                        NewsStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsStatusesGlobals", t => t.NewsStatus_Id)
                .Index(t => t.NewsStatus_Id);
            
            CreateTable(
                "dbo.ExtrasLocals",
                c => new
                    {
                        ExtrasGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Subtitle = c.String(),
                        Body_text = c.String(),
                    })
                .PrimaryKey(t => new { t.ExtrasGlobalId, t.LanguageId })
                .ForeignKey("dbo.ExtrasGlobals", t => t.ExtrasGlobalId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ExtrasGlobalId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        stream_id = c.Guid(nullable: false),
                        file_stream = c.Binary(),
                        name = c.String(),
                        file_type = c.String(),
                    })
                .PrimaryKey(t => t.stream_id);
            
            CreateTable(
                "Log4net.Logs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Thread = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        isActive = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubMenus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                        isActive = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                        Menu_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.Menu_Id)
                .Index(t => t.Menu_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessagesGlobals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PublishingDate = c.DateTime(),
                        NewsStatus_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewsStatusesGlobals", t => t.NewsStatus_Id)
                .Index(t => t.NewsStatus_Id);
            
            CreateTable(
                "dbo.MessagesLocals",
                c => new
                    {
                        MessagesGlobalId = c.Guid(nullable: false),
                        LanguageId = c.Guid(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => new { t.MessagesGlobalId, t.LanguageId })
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.MessagesGlobals", t => t.MessagesGlobalId, cascadeDelete: true)
                .Index(t => t.LanguageId)
                .Index(t => t.MessagesGlobalId);
            
            CreateTable(
                "dbo.SystemParameters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        Public = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        isLocked = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TagsGlobalNewsGlobals",
                c => new
                    {
                        TagsGlobal_Id = c.Guid(nullable: false),
                        NewsGlobal_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagsGlobal_Id, t.NewsGlobal_Id })
                .ForeignKey("dbo.TagsGlobals", t => t.TagsGlobal_Id, cascadeDelete: true)
                .ForeignKey("dbo.NewsGlobals", t => t.NewsGlobal_Id, cascadeDelete: true)
                .Index(t => t.TagsGlobal_Id)
                .Index(t => t.NewsGlobal_Id);
            
            CreateTable(
                "dbo.ApplicationRoleSubMenus",
                c => new
                    {
                        ApplicationRole_Id = c.String(nullable: false, maxLength: 128),
                        SubMenu_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationRole_Id, t.SubMenu_Id })
                .ForeignKey("dbo.AspNetRoles", t => t.ApplicationRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubMenus", t => t.SubMenu_Id, cascadeDelete: true)
                .Index(t => t.ApplicationRole_Id)
                .Index(t => t.SubMenu_Id);

            Sql("--GO");
            Sql("insert AspNetRoles (Id, Name, Discriminator) values('4A900255-0B4E-4BF6-AB69-22AFE3C11BF5', 'SysAdmin', 'ApplicationRole')");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MessagesGlobals", "NewsStatus_Id", "dbo.NewsStatusesGlobals");
            DropForeignKey("dbo.MessagesLocals", "MessagesGlobalId", "dbo.MessagesGlobals");
            DropForeignKey("dbo.MessagesLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.SubMenus", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.ApplicationRoleSubMenus", "SubMenu_Id", "dbo.SubMenus");
            DropForeignKey("dbo.ApplicationRoleSubMenus", "ApplicationRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ExtrasGlobals", "NewsStatus_Id", "dbo.NewsStatusesGlobals");
            DropForeignKey("dbo.ExtrasLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ExtrasLocals", "ExtrasGlobalId", "dbo.ExtrasGlobals");
            DropForeignKey("dbo.CategoriesGlobals", "ExtrasGlobal_Id", "dbo.ExtrasGlobals");
            DropForeignKey("dbo.EventsGlobals", "NewsStatus_Id", "dbo.NewsStatusesGlobals");
            DropForeignKey("dbo.EventsLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.EventsLocals", "EventsGlobalId", "dbo.EventsGlobals");
            DropForeignKey("dbo.TagsGlobals", "ContactsGlobal_Id", "dbo.ContactsGlobals");
            DropForeignKey("dbo.ContactsGlobals", "Photo_Id", "dbo.AttachedFiles");
            DropForeignKey("dbo.ContactsGlobals", "NewsStatus_Id", "dbo.NewsStatusesGlobals");
            DropForeignKey("dbo.NewsStatusesLocals", "NewsStatusGlobalId", "dbo.NewsStatusesGlobals");
            DropForeignKey("dbo.NewsStatusesLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.NewsGlobals", "Thumbnail_Id", "dbo.AttachedFiles");
            DropForeignKey("dbo.TagsLocals", "TagsGlobalId", "dbo.TagsGlobals");
            DropForeignKey("dbo.TagsLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.TagsGlobalNewsGlobals", "NewsGlobal_Id", "dbo.NewsGlobals");
            DropForeignKey("dbo.TagsGlobalNewsGlobals", "TagsGlobal_Id", "dbo.TagsGlobals");
            DropForeignKey("dbo.NewsGlobals", "NewsStatus_Id", "dbo.NewsStatusesGlobals");
            DropForeignKey("dbo.NewsLocals", "NewsGlobalId", "dbo.NewsGlobals");
            DropForeignKey("dbo.NewsLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.NewsGlobals", "Headline_picture_Id", "dbo.AttachedFiles");
            DropForeignKey("dbo.ContactsLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.ContactsLocals", "ContactsGlobalId", "dbo.ContactsGlobals");
            DropForeignKey("dbo.CategoriesLocals", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.CategoriesLocals", "CategoriesGlobalId", "dbo.CategoriesGlobals");
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.MessagesGlobals", new[] { "NewsStatus_Id" });
            DropIndex("dbo.MessagesLocals", new[] { "MessagesGlobalId" });
            DropIndex("dbo.MessagesLocals", new[] { "LanguageId" });
            DropIndex("dbo.SubMenus", new[] { "Menu_Id" });
            DropIndex("dbo.ApplicationRoleSubMenus", new[] { "SubMenu_Id" });
            DropIndex("dbo.ApplicationRoleSubMenus", new[] { "ApplicationRole_Id" });
            DropIndex("dbo.ExtrasGlobals", new[] { "NewsStatus_Id" });
            DropIndex("dbo.ExtrasLocals", new[] { "LanguageId" });
            DropIndex("dbo.ExtrasLocals", new[] { "ExtrasGlobalId" });
            DropIndex("dbo.CategoriesGlobals", new[] { "ExtrasGlobal_Id" });
            DropIndex("dbo.EventsGlobals", new[] { "NewsStatus_Id" });
            DropIndex("dbo.EventsLocals", new[] { "LanguageId" });
            DropIndex("dbo.EventsLocals", new[] { "EventsGlobalId" });
            DropIndex("dbo.TagsGlobals", new[] { "ContactsGlobal_Id" });
            DropIndex("dbo.ContactsGlobals", new[] { "Photo_Id" });
            DropIndex("dbo.ContactsGlobals", new[] { "NewsStatus_Id" });
            DropIndex("dbo.NewsStatusesLocals", new[] { "NewsStatusGlobalId" });
            DropIndex("dbo.NewsStatusesLocals", new[] { "LanguageId" });
            DropIndex("dbo.NewsGlobals", new[] { "Thumbnail_Id" });
            DropIndex("dbo.TagsLocals", new[] { "TagsGlobalId" });
            DropIndex("dbo.TagsLocals", new[] { "LanguageId" });
            DropIndex("dbo.TagsGlobalNewsGlobals", new[] { "NewsGlobal_Id" });
            DropIndex("dbo.TagsGlobalNewsGlobals", new[] { "TagsGlobal_Id" });
            DropIndex("dbo.NewsGlobals", new[] { "NewsStatus_Id" });
            DropIndex("dbo.NewsLocals", new[] { "NewsGlobalId" });
            DropIndex("dbo.NewsLocals", new[] { "LanguageId" });
            DropIndex("dbo.NewsGlobals", new[] { "Headline_picture_Id" });
            DropIndex("dbo.ContactsLocals", new[] { "LanguageId" });
            DropIndex("dbo.ContactsLocals", new[] { "ContactsGlobalId" });
            DropIndex("dbo.CategoriesLocals", new[] { "LanguageId" });
            DropIndex("dbo.CategoriesLocals", new[] { "CategoriesGlobalId" });
            DropTable("dbo.ApplicationRoleSubMenus");
            DropTable("dbo.TagsGlobalNewsGlobals");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SystemParameters");
            DropTable("dbo.MessagesLocals");
            DropTable("dbo.MessagesGlobals");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SubMenus");
            DropTable("dbo.Menus");
            DropTable("Log4net.Logs");
            DropTable("dbo.Files");
            DropTable("dbo.ExtrasLocals");
            DropTable("dbo.ExtrasGlobals");
            DropTable("dbo.EventsLocals");
            DropTable("dbo.EventsGlobals");
            DropTable("dbo.NewsStatusesLocals");
            DropTable("dbo.TagsLocals");
            DropTable("dbo.TagsGlobals");
            DropTable("dbo.NewsLocals");
            DropTable("dbo.NewsGlobals");
            DropTable("dbo.NewsStatusesGlobals");
            DropTable("dbo.ContactsLocals");
            DropTable("dbo.ContactsGlobals");
            DropTable("dbo.Languages");
            DropTable("dbo.CategoriesLocals");
            DropTable("dbo.CategoriesGlobals");
            DropTable("dbo.AttachedFiles");
        }
    }
}
