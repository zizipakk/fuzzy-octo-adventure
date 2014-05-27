namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iosproductiontosetup : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("insert SystemParameters (Id, Name, Value, [Description], [Public]) values ('19E07FEF-B583-4DA2-83C6-235B3F7B1B72', 'IOS Production', 'false', '', 1)"));
        }
        
        public override void Down()
        {
        }
    }
}
