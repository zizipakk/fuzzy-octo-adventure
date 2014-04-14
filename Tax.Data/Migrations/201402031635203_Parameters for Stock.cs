namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParametersforStock : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            //helyszínek
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('CBFF9936-20B6-41CB-927F-FA3FE9655F98','Fejér')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('CE4F9338-AE9C-4274-915F-722385313DD9','Bács-Kiskun')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('43160955-D31A-434B-ACF9-A5BD15AF1112','Baranya')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('7F869778-02AA-4650-AF4D-A192E63EB405','Békés')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('35599BDD-BA7A-464E-86A9-5073B36A0BDE','Csongrád')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('2F453CDF-ED6D-403A-91B8-1205E454C17E','Jász-Nagykun-Szolnok')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('A3ABB4D7-3A93-4E98-BD01-DC370E3EF616','Nógrád')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('CDC9AB2D-5D7B-4E5F-AF0F-7D20276AA51F','Somogy')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('0786B3C6-358A-4233-B156-4EA3EB175843','Szabolcs-Szatmár-Bereg')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('0D5A5E1A-07CD-4E41-B7D6-1DAAD89F7BB6','Veszprém')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('A5A1464D-C8DF-4303-9841-A51E26C705D7','Zala')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('380925D1-AB14-4127-B9DF-58BADFA522CB','Szerviz')"));
            Sql(string.Format("INSERT Areas (Id,AreaName) VALUES('CD45263E-3C88-4DDC-8B1C-CFEDA55C5329','Ügyfél')"));

            //Eszköztípusok
            Sql(string.Format("INSERT DeviceTypes (Id,DeviceTypeName) VALUES('C311005E-9A77-49EA-884A-52CE7277DE78','Táblagép')"));
            Sql(string.Format("INSERT DeviceTypes (Id,DeviceTypeName) VALUES('8AD9A9E7-4FFD-43CB-BDBA-0FA76DB9B0A0','Tartozék')"));

            //Eszközállapot
            Sql(string.Format("INSERT DeviceStatus (Id,DeviceStatusName) VALUES('3A290A80-861C-44D8-A21A-0A2478A2CCEB','Mûködõképes')"));
            Sql(string.Format("INSERT DeviceStatus (Id,DeviceStatusName) VALUES('C50F4E74-1A25-433E-8D7E-A7DCD7603B63','Selejt')"));
        }

        public override void Down()
        {
        }
    }
}
