namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyaccountperiods : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //AccountPeriod
            Sql(string.Format("update AccountPeriods set AccountPeriodStatus_Id = '5B79F603-D931-44EF-9CBD-7FCA16B33B39' where Id = '1671F6B7-0FF6-4E55-BCBD-A4916ECA9983'"));
            Sql(string.Format("update AccountPeriods set PeriodBegin = '2014.02.01', PeriodEnd = '2014.02.28', AccountPeriodStatus_Id = '5B79F603-D931-44EF-9CBD-7FCA16B33B39' where Id = '4B660FB9-AA50-4155-819B-6DD2B0B36AAB'"));
            Sql(string.Format("update AccountPeriods set PeriodBegin = '2014.03.01', PeriodEnd = '2014.03.31', AccountPeriodStatus_Id = '5B79F603-D931-44EF-9CBD-7FCA16B33B39' where Id = '0E26CB27-F4F2-449F-BEBF-3474006A0B56'"));
            Sql(string.Format("update AccountPeriods set PeriodBegin = '2014.04.01', PeriodEnd = '2014.04.30', AccountPeriodStatus_Id = '5B79F603-D931-44EF-9CBD-7FCA16B33B39' where Id = '189C9E1F-7FD6-417A-B607-862F87C7A7F0'"));
        }
        
        public override void Down()
        {
        }
    }
}
