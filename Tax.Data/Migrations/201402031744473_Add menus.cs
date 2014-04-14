namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addmenus : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("insert Menus (Id,Name,Controller,Action,isActive,Position) values ('C66A7876-06D1-40DB-9A12-48627EE05708','Készlet','','',1,1)"));

            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('1C8CD53B-593A-438F-8875-99214FE1DAC0','Eszközök követése','Stock','Devices',1,1,'C66A7876-06D1-40DB-9A12-48627EE05708')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('CDB2D464-6D77-4CC2-AA1E-7012629929EC','Ügyfelek követése','Stock','Users',1,2,'C66A7876-06D1-40DB-9A12-48627EE05708')"));

            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('1C8CD53B-593A-438F-8875-99214FE1DAC0','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('1C8CD53B-593A-438F-8875-99214FE1DAC0','c8ca4509-e8c4-49f3-b2aa-df0ea46127be')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('CDB2D464-6D77-4CC2-AA1E-7012629929EC','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('CDB2D464-6D77-4CC2-AA1E-7012629929EC','c8ca4509-e8c4-49f3-b2aa-df0ea46127be')"));

        }
        
        public override void Down()
        {
        }
    }
}
