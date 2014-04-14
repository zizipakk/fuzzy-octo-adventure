namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pumpsystemparameters : DbMigration
    {
        public override void Up()
        {
            //nyit�s
            Sql(string.Format("--GO"));
            //systemparameters
            Sql(string.Format("INSERT SystemParameters (Id,Name,Value,[Description],[Public]) VALUES('9C5335FE-8EC9-4F16-8D47-18AED19C5B23','El�regiszt�ci� folyamatban (1/0)?','0','1 �rt�k eset�n �t�ll a rendszer el�regiszt�rci�s �zembe a norm�lis helyett!',0)"));
        }

        public override void Down()
        {
            //nyit�s
            Sql(string.Format("--GO"));
            //SocialPositions
            Sql(string.Format("DELETE SystemParameters WHERE Id = '9C5335FE-8EC9-4F16-8D47-18AED19C5B23'"));
        }
    }
}
