namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        KontaktUserRole_Id = c.Guid(),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.KontaktUserRoles", t => t.KontaktUserRole_Id)
                .Index(t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.KontaktUserRole_Id);
            
            CreateTable(
                "dbo.KontaktUserRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InterpreterCenter_Id = c.Guid(),
                        PBXExtensionData_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterpreterCenters", t => t.InterpreterCenter_Id)
                .ForeignKey("dbo.PBXExtensionDatas", t => t.PBXExtensionData_Id)
                .Index(t => t.InterpreterCenter_Id)
                .Index(t => t.PBXExtensionData_Id);
            
            CreateTable(
                "dbo.InterpreterCenters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InterpreterCenterAddress = c.String(),
                        Area_Id = c.Guid(),
                        Postcode_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.Area_Id)
                .ForeignKey("dbo.Postcodes", t => t.Postcode_Id)
                .Index(t => t.Area_Id)
                .Index(t => t.Postcode_Id);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AreaName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Postcodes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        City = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PBXExtensionDatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExtensionID = c.String(),
                        Password = c.String(),
                        isDroped = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        PhoneNumber_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.PhoneNumbers", t => t.PhoneNumber_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.PhoneNumber_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Email = c.String(),
                        isLocked = c.Boolean(),
                        isEmailValidated = c.Boolean(),
                        Password = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        KontaktUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KontaktUsers", t => t.KontaktUser_Id)
                .Index(t => t.KontaktUser_Id);
            
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
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
                "dbo.KontaktUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        isSinoszMember = c.Boolean(nullable: false),
                        SinuszId = c.String(),
                        isCommunicationRequested = c.Boolean(nullable: false),
                        isDeviceReqested = c.Boolean(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhoneNumbers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InnerPhoneNumber = c.String(),
                        ExternalPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.SinoszUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SinuszId = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ValidUntil = c.DateTime(nullable: false),
                        ValidateDate = c.DateTime(),
                        Code = c.String(),
                        TokenTarget = c.Int(nullable: false),
                        TargetId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubMenuKontaktRoles",
                c => new
                    {
                        SubMenu_Id = c.Guid(nullable: false),
                        KontaktRole_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubMenu_Id, t.KontaktRole_Id })
                .ForeignKey("dbo.SubMenus", t => t.SubMenu_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.KontaktRole_Id, cascadeDelete: true)
                .Index(t => t.SubMenu_Id)
                .Index(t => t.KontaktRole_Id);

            Sql("--GO");
            //Roles
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('323dcd28-4096-450a-ad93-77e1739ba204','SysAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('3ec3861d-98c3-404c-a664-767f3651c307','Ügyfél','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('bf07d7ef-a40f-488e-9e82-69eb2357cd11','Jeltolmácsok','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('ae9c9735-71e6-48ea-b2a6-a9ecf07e395c','Szakmai vezetõ','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('af85bf54-7f32-4fae-b003-3ff9d3fea603','Diszpécser','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('283bd99b-5ce5-4038-a469-54acefc9ab1a','Üzemeltetõ','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('cd64caa7-6d0b-4c4a-8847-a23c16cdd048','SinoszAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('7efffd2e-a367-46c3-b0d9-458eeca046c1','SinoszUser','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('49ce4c30-0946-4e6f-b7ec-cc3eefef25b0','ServiceAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('b358102c-5a0b-49e5-9568-da33789f37dc','ServicePoweruser','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('598c1b87-4898-4c7b-84d6-9380479fca95','ServiceUser','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('21513011-5a85-499b-9b54-eea8427b0aeb','OperAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('deb6d88c-788e-4aa1-bcfd-f293e7f431cf','OperUser','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('743ab24a-9492-4597-99a7-71df482f0e8b','SupportAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('b006fa23-9c25-4d07-9670-fff8eb196cea','SupportUser','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('c8ca4509-e8c4-49f3-b2aa-df0ea46127be','DeviceAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('cd01df1e-5552-4cca-a104-d87ac8268859','AccountAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('5a5da4b3-6497-4442-a98b-0b15316b3ee9','ReportAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('cbabfa30-b08e-4166-990f-503157382c27','KontaktAdmin','KontaktRole')"));
            Sql(string.Format("insert AspNetRoles (Id,Name,Discriminator) values ('61dbb168-970f-4250-8129-0a4de3e101c3','PBXUser','KontaktRole')"));

            //Menu
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('9d941e8a-5dda-401e-8967-e8227efa6599','Adminisztráció','Index','Admin',1,1)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('b7d426c9-b03b-4195-8382-2721b8b9f769','SINOSZ-nyilvántartás','Index','Sinosz',1,2)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('1f2f9271-d810-478e-9ae7-0be5667a9127','Szolgáltatásszervezés','Index','Service',1,3)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('e703d238-8506-42d6-b755-eed5779f3cdd','Panaszkezelés','Index','Support',1,4)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('44d6d88d-d2ef-4307-8a4e-88a42937d64f','Készlet','Index','Device',1,5)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('0e5b10ae-b214-4f9e-bc7a-7771cd097f75','Pénzügy','Index','Account',1,6)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('2e1a46a6-d4c1-4782-92dc-854ea0762650','Riport','Index','Report',1,7)"));
            Sql(string.Format("insert Menus (Id,Name,Action,Controller,isActive,Position) values ('7541f5a9-209d-45a3-8647-6e1b6171e248','KONTAKT Portál','Index','Kontakt',1,8)"));

            //PostCode
            Sql(string.Format("insert PostCodes (Id,Code,City,Country) values ('d116ad63-f35b-4726-b047-c09f296702e1','1111','Budapest','Budapest')"));

            //Area
            Sql(string.Format("insert Areas (Id,AreaName) values ('266dccb5-9f39-4b1c-a854-d057c7daa305','Központ')"));

            //InterpreterCenter
            Sql(string.Format("insert InterpreterCenters (Id,InterpreterCenterAddress,Area_Id,Postcode_Id) values ('8c386aae-7030-4265-b145-971a7da44172','Fõ utca 1.','266dccb5-9f39-4b1c-a854-d057c7daa305','d116ad63-f35b-4726-b047-c09f296702e1')"));
            Sql(string.Format("insert InterpreterCenters (Id,InterpreterCenterAddress,Area_Id,Postcode_Id) values ('7aabcfdb-22ce-4a2f-9a43-650b4d2fcaaa','Fõ utca 2.','266dccb5-9f39-4b1c-a854-d057c7daa305','d116ad63-f35b-4726-b047-c09f296702e1')"));
            Sql(string.Format("insert InterpreterCenters (Id,InterpreterCenterAddress,Area_Id,Postcode_Id) values ('a3d3fde2-7d1f-4b5e-bd6f-c9b83c5b1479','Fõ utca 3.','266dccb5-9f39-4b1c-a854-d057c7daa305','d116ad63-f35b-4726-b047-c09f296702e1')"));

            //PhoneNumber
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('73C30F69-9A5D-4906-B9E3-73772EEEEEE3','8880000','8880000')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('20D3F787-FB18-466B-94B0-9B9D2DD9FF1E','8880001','8880001')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('EA5F3746-F216-4DFD-A12A-A6D0B058F26C','8880002','8880002')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('1BDBC78F-9DB8-4C51-959E-220850A0421B','8880003','8880003')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('02E2F547-2B54-41F4-A1D8-2E12E7DAB793','8880004','8880004')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('FF54EF20-A483-4A90-8D76-96CF1848BFD7','8880005','8880005')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('6FAFD937-36A3-44C5-B6C0-43EAA7C06E37','8880006','8880006')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('53EE9275-A47E-461A-8558-619E9C3814EB','8880007','8880007')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('09651726-97CF-48F9-9EFE-22A89C56CF3E','8880008','8880008')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('7414730E-67B4-42F0-9DB3-3D98CC845215','8880009','8880009')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('61572684-DF5C-4908-B6C4-76BD01C70453','8880010','8880010')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('FF6201FB-AAFA-4FB5-A908-83018A305C0B','8880011','8880011')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('787B24B1-DA4A-48B7-819E-63B65697ABF5','8880012','8880012')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('BEDE72B9-A1A9-4060-BC64-1FEC9236566C','8880013','8880013')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('1CC5294C-5494-4288-88D3-CA38175CF8F2','8880014','8880014')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('EE969DE7-A56F-4A54-8C0B-047A34E608FD','8880015','8880015')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('06168817-3A3A-4CF7-872C-7A3BAD052EF9','8880016','8880016')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('820BB08A-BD06-48DF-9D83-7A2827A8970B','8880017','8880017')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('559C9029-47DC-47A0-AE73-5597A094A3A2','8880018','8880018')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('E9BDD0F0-A1B2-4567-99E6-83C4848E6868','8880019','8880019')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('175B39EA-435D-4508-9850-AD3D5E937537','8880020','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('57CC8960-9EF3-436A-88A1-732DE449D90D','8880021','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('B9A7AB08-F90B-4234-B791-516AEDDE2A1B','8880022','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('4E4D1B16-4F09-41C6-B69E-37F4AEC34D51','8880023','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('A10F87E5-3015-47F3-9E86-C6398223CBAA','8880024','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('291448E1-7CFC-4DB0-BC9C-6AACBADC6A88','8880025','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('327141C5-2ADD-4C6A-A44F-E3F8FD87147B','8880026','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('589D913F-C2A6-4631-BBCB-1FB4A0AB0429','8880027','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('98F53F3B-4A86-429D-BDE6-E174D97C3B4E','8880028','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('47F27A36-F67E-4DED-BB96-1BD65BE3F6C7','8880029','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('1716D866-FB91-4DC7-AFFE-EFAF0B614A5A','8880030','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('BEBED336-BD09-4C6B-B98D-18641DA18A95','8880031','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('B582C687-35DA-4E3E-AF58-00B8C89B1FBF','8880032','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('886FC60B-0FD6-4DA3-A98C-8919814185CE','8880033','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('88098AA4-BCB8-41C2-95B1-B74706225329','8880034','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('40160A70-F51A-496D-A257-C299373B69C4','8880035','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('360DDDCA-CB12-4F96-86AF-162F32093A0B','8880036','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('367F30E3-5703-46A3-8A1F-F0C0CBBCC625','8880037','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('980AD770-2E7A-4DA9-A3CF-1B2A3002F19C','8880038','')"));
            //Sql(string.Format("insert PhoneNumbers (Id,InnerPhoneNumber,ExternalPhoneNumber) values ('BAE63EB3-E6E1-4CE2-B000-5CAFA8CDBAB4','8880039','')"));

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SinoszUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubMenus", "Menu_Id", "dbo.Menus");
            DropForeignKey("dbo.AspNetUserRoles", "KontaktUserRole_Id", "dbo.KontaktUserRoles");
            DropForeignKey("dbo.KontaktUserRoles", "PBXExtensionData_Id", "dbo.PBXExtensionDatas");
            DropForeignKey("dbo.PBXExtensionDatas", "PhoneNumber_Id", "dbo.PhoneNumbers");
            DropForeignKey("dbo.PBXExtensionDatas", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "KontaktUser_Id", "dbo.KontaktUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SubMenuKontaktRoles", "KontaktRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.SubMenuKontaktRoles", "SubMenu_Id", "dbo.SubMenus");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InterpreterCenters", "Postcode_Id", "dbo.Postcodes");
            DropForeignKey("dbo.KontaktUserRoles", "InterpreterCenter_Id", "dbo.InterpreterCenters");
            DropForeignKey("dbo.InterpreterCenters", "Area_Id", "dbo.Areas");
            DropIndex("dbo.SinoszUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SubMenus", new[] { "Menu_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "KontaktUserRole_Id" });
            DropIndex("dbo.KontaktUserRoles", new[] { "PBXExtensionData_Id" });
            DropIndex("dbo.PBXExtensionDatas", new[] { "PhoneNumber_Id" });
            DropIndex("dbo.PBXExtensionDatas", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "KontaktUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.SubMenuKontaktRoles", new[] { "KontaktRole_Id" });
            DropIndex("dbo.SubMenuKontaktRoles", new[] { "SubMenu_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.InterpreterCenters", new[] { "Postcode_Id" });
            DropIndex("dbo.KontaktUserRoles", new[] { "InterpreterCenter_Id" });
            DropIndex("dbo.InterpreterCenters", new[] { "Area_Id" });
            DropTable("dbo.SubMenuKontaktRoles");
            DropTable("dbo.Tokens");
            DropTable("dbo.SinoszUsers");
            DropTable("dbo.Menus");
            DropTable("Log4net.Logs");
            DropTable("dbo.PhoneNumbers");
            DropTable("dbo.KontaktUsers");
            DropTable("dbo.SubMenus");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PBXExtensionDatas");
            DropTable("dbo.Postcodes");
            DropTable("dbo.Areas");
            DropTable("dbo.InterpreterCenters");
            DropTable("dbo.KontaktUserRoles");
            DropTable("dbo.AspNetUserRoles");
        }
    }
}
