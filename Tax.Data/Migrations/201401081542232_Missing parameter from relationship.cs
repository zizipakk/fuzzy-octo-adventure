namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Missingparameterfromrelationship : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('5AFCAD8E-1B14-4F45-81DC-48396F6449C8','Nincs')"));
        }

        public override void Down()
        {
        }
    }
}
