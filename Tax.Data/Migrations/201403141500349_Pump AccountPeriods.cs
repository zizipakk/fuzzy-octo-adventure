namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PumpAccountPeriods : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //AccountPeriodStatus
            Sql(string.Format("insert AccountPeriodStatus (Id,StatusName) values ('5B79F603-D931-44EF-9CBD-7FCA16B33B39','Nyitott')"));
            Sql(string.Format("insert AccountPeriodStatus (Id,StatusName) values ('2C706B28-998E-4A06-A3BE-7A031B1CDDEA','Zárt')"));
            //AccountPeriod
            Sql(string.Format("insert AccountPeriods (Id,PeriodBegin,PeriodEnd) values ('1671F6B7-0FF6-4E55-BCBD-A4916ECA9983','2014.01.01','2014.01.31')"));
            Sql(string.Format("insert AccountPeriods (Id,PeriodBegin,PeriodEnd) values ('4B660FB9-AA50-4155-819B-6DD2B0B36AAB','2014.01.01','2014.01.31')"));
            Sql(string.Format("insert AccountPeriods (Id,PeriodBegin,PeriodEnd) values ('0E26CB27-F4F2-449F-BEBF-3474006A0B56','2014.01.01','2014.01.31')"));
            Sql(string.Format("insert AccountPeriods (Id,PeriodBegin,PeriodEnd) values ('189C9E1F-7FD6-417A-B607-862F87C7A7F0','2014.01.01','2014.01.31')"));
            //PBXExtensionDatas
            Sql(string.Format("update PBXExtensionDatas set LimitType_Id = 'EAED4816-72A2-4C07-964B-569557BB58C7'"));
            
        }

        public override void Down()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //AccountPeriodStatus
            Sql(string.Format("delete AccountPeriodStatus where Id = '5B79F603-D931-44EF-9CBD-7FCA16B33B39'"));            
            Sql(string.Format("delete AccountPeriodStatus where Id = '2C706B28-998E-4A06-A3BE-7A031B1CDDEA'"));
            //AccountPeriod
            Sql(string.Format("delete AccountPeriods where Id = '1671F6B7-0FF6-4E55-BCBD-A4916ECA9983'"));
            Sql(string.Format("delete AccountPeriods where Id = '4B660FB9-AA50-4155-819B-6DD2B0B36AAB'"));
            Sql(string.Format("delete AccountPeriods where Id = '0E26CB27-F4F2-449F-BEBF-3474006A0B56'"));
            Sql(string.Format("delete AccountPeriods where Id = '189C9E1F-7FD6-417A-B607-862F87C7A7F0'"));             
            //PBXExtensionDatas
            Sql(string.Format("update PBXExtensionDatas set LimitType_Id = NULL"));
        }
    }
}
