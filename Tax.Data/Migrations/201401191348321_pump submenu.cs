namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pumpsubmenu : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('DB7B41D7-6A0A-45C8-AA8F-264BC63C4879','Rendszer-paraméterek','Admin','Setup',1,1,'9D941E8A-5DDA-401E-8967-E8227EFA6599')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('B154A64C-CA2E-4241-A8CB-F528C3F1E336','Rendszer-paraméterek','Admin','Log',1,2,'9D941E8A-5DDA-401E-8967-E8227EFA6599')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('2EF6688F-32A1-4EDF-AE82-84B99E1B9CE8','Rendszer-paraméterek','Admin','Rule',1,3,'9D941E8A-5DDA-401E-8967-E8227EFA6599')"));
            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('3F70F733-92C9-4F1A-AE0C-6A2D9C2D3404','Rendszer-paraméterek','Admin','Trunk',1,4,'9D941E8A-5DDA-401E-8967-E8227EFA6599')"));

            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('DB7B41D7-6A0A-45C8-AA8F-264BC63C4879','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('B154A64C-CA2E-4241-A8CB-F528C3F1E336','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('2EF6688F-32A1-4EDF-AE82-84B99E1B9CE8','323dcd28-4096-450a-ad93-77e1739ba204')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('3F70F733-92C9-4F1A-AE0C-6A2D9C2D3404','323dcd28-4096-450a-ad93-77e1739ba204')"));                    
        }
        
        public override void Down()
        {
        }
    }
}
