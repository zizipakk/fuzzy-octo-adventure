namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnemorefieldtoPrices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "ValidityBegin", c => c.DateTime(nullable: false));
            //nyitás
            Sql(string.Format("--GO"));
            //Price
            Sql(string.Format("update Prices set ValidityBegin = '2013.12.01' where Id = 'DA2A4AD4-C12B-4EA3-96B7-DF9FA2A96244'"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "ValidityBegin");
        }
    }
}
