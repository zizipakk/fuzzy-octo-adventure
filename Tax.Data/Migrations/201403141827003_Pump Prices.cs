namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PumpPrices : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //Price
            Sql(string.Format("insert Prices (Id,ValidityEnd,Sum,VAT) values ('DA2A4AD4-C12B-4EA3-96B7-DF9FA2A96244','9999.12.31',0,0.27)"));
        }
        
        public override void Down()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //Price
            Sql(string.Format("delete Prices where Id = 'DA2A4AD4-C12B-4EA3-96B7-DF9FA2A96244'"));     
        }
    }
}
