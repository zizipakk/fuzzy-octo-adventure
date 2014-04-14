namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmenusforSinosz : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("update SubMenus set Name = 'Naplók' where Id = 'B154A64C-CA2E-4241-A8CB-F528C3F1E336'"));
            Sql(string.Format("update SubMenus set Name = 'Jogosultságok' where Id = '2EF6688F-32A1-4EDF-AE82-84B99E1B9CE8'"));
            Sql(string.Format("update SubMenus set Name = 'Törzsadatok' where Id = '3F70F733-92C9-4F1A-AE0C-6A2D9C2D3404'"));
            
            //Sinosz
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('87F75D8D-FA57-4DED-B35E-B2287490FC59','Sinosz Hírek','Sinosz','New',1,1,'B7D426C9-B03B-4195-8382-2721B8B9F769')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('AAE5C282-99D4-48A4-A35D-F192CFFF1DDF','Tagnyilvántartás','Sinosz','Index',1,2,'B7D426C9-B03B-4195-8382-2721B8B9F769')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('12E8EFC4-7356-4623-996D-E7FC208EA46B','Listák','Sinosz','List',1,3,'B7D426C9-B03B-4195-8382-2721B8B9F769')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('D697C83E-F8B1-4DA7-9988-2D4FBE1EFC01','Rendszerbeállítások','Sinosz','Parameter',1,4,'B7D426C9-B03B-4195-8382-2721B8B9F769')"));

            //SysAdmin
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('87F75D8D-FA57-4DED-B35E-B2287490FC59','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('AAE5C282-99D4-48A4-A35D-F192CFFF1DDF','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('12E8EFC4-7356-4623-996D-E7FC208EA46B','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('D697C83E-F8B1-4DA7-9988-2D4FBE1EFC01','323dcd28-4096-450a-ad93-77e1739ba204')"));
            //SinoszAdmin
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('87F75D8D-FA57-4DED-B35E-B2287490FC59','cd64caa7-6d0b-4c4a-8847-a23c16cdd048')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('AAE5C282-99D4-48A4-A35D-F192CFFF1DDF','cd64caa7-6d0b-4c4a-8847-a23c16cdd048')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('12E8EFC4-7356-4623-996D-E7FC208EA46B','cd64caa7-6d0b-4c4a-8847-a23c16cdd048')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('D697C83E-F8B1-4DA7-9988-2D4FBE1EFC01','cd64caa7-6d0b-4c4a-8847-a23c16cdd048')"));
            //SysUser
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('87F75D8D-FA57-4DED-B35E-B2287490FC59','7efffd2e-a367-46c3-b0d9-458eeca046c1')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('AAE5C282-99D4-48A4-A35D-F192CFFF1DDF','7efffd2e-a367-46c3-b0d9-458eeca046c1')"));
        }
        
        public override void Down()
        {
        }
    }
}
