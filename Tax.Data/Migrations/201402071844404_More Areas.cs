namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreAreas : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //Areas
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('59AC6F70-CCC3-41AA-B1C5-C2C66D030110','Borsod-Abaúj-Zemplén')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('CEA43D78-F6EF-4F3A-A7DE-4B750F0D040C','Gyõr-Moson-Sopron')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('78495B71-351F-40F9-B7C4-FA355CFF6597','Hajdú-Bihar')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('77CE56EC-F2CC-4201-BE32-F31861885E35','Heves')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('2AFDD97F-D2FF-4568-8402-CF7C95C0ED31','Budapest')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('1EC4DF66-7F7D-4B65-9F58-B7E638B5A885','Komárom-Esztergom')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('8529D07A-2CDA-4162-8431-C23C91151F82','Pest')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('34125765-F435-409F-A8A2-C7D25F4E0B57','Vas')"));
        }
        
        public override void Down()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //Areas
            Sql(string.Format("DELETE Areas WHERE Id = '59AC6F70-CCC3-41AA-B1C5-C2C66D030110'"));
            Sql(string.Format("DELETE Areas WHERE Id = 'CEA43D78-F6EF-4F3A-A7DE-4B750F0D040C'"));
            Sql(string.Format("DELETE Areas WHERE Id = '78495B71-351F-40F9-B7C4-FA355CFF6597'"));
            Sql(string.Format("DELETE Areas WHERE Id = '77CE56EC-F2CC-4201-BE32-F31861885E35'"));
            Sql(string.Format("DELETE Areas WHERE Id = '2AFDD97F-D2FF-4568-8402-CF7C95C0ED31'"));
            Sql(string.Format("DELETE Areas WHERE Id = '1EC4DF66-7F7D-4B65-9F58-B7E638B5A885'"));
            Sql(string.Format("DELETE Areas WHERE Id = '8529D07A-2CDA-4162-8431-C23C91151F82'"));
            Sql(string.Format("DELETE Areas WHERE Id = '34125765-F435-409F-A8A2-C7D25F4E0B57'"));
        }
    }
}
