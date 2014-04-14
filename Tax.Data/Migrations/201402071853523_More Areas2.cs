namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreAreas2 : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //Areas
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('E3B32BA0-97E4-466A-8213-AFD735846D0D','Tolna')"));
        }

        public override void Down()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //Areas
            Sql(string.Format("DELETE Areas WHERE Id = 'E3B32BA0-97E4-466A-8213-AFD735846D0D'"));
         }
    }
}
