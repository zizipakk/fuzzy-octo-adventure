namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pumplimits : DbMigration
    {
        public override void Up()
        {
            //nyit�s
            Sql(string.Format("--GO"));

            Sql(string.Format("insert LimitTypes (Id,LimitName,MaxLimitPerYear,MinLimitPerQyear) values ('EAED4816-72A2-4C07-964B-569557BB58C7','Tesztel�i limit',120.0, 10.0)"));            
        }
        
        public override void Down()
        {
            //nyit�s
            Sql(string.Format("--GO"));

            Sql(string.Format("delete LimitTypes where Id = 'EAED4816-72A2-4C07-964B-569557BB58C7'"));            
        }
    }
}
