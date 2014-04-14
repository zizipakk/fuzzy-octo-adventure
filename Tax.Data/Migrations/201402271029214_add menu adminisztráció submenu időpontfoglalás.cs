namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmenuadminisztrációsubmenuidőpontfoglalás : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));

            Sql(string.Format("insert SubMenus (Id,Name,Controller,Action,isActive,Position,Menu_Id) values ('A465946B-C9FA-482F-9B5B-0F0C293C7ABC','Időpontfoglalás','Stock','ReservationAdmin',1,5,'9D941E8A-5DDA-401E-8967-E8227EFA6599')"));
            Sql(string.Format("insert SubMenuKontaktRoles (SubMenu_Id,KontaktRole_Id) values ('A465946B-C9FA-482F-9B5B-0F0C293C7ABC','323dcd28-4096-450a-ad93-77e1739ba204')"));
        }

        public override void Down()
        {
            //nyitás
            Sql(string.Format("--GO"));

            Sql(string.Format("delete SubMenuKontaktRoles where SubMenu_Id = 'A465946B-C9FA-482F-9B5B-0F0C293C7ABC' and KontaktRole_Id = '323dcd28-4096-450a-ad93-77e1739ba204'"));
            Sql(string.Format("delete SubMenus where Id = 'A465946B-C9FA-482F-9B5B-0F0C293C7ABC'"));

        }
    }
}
