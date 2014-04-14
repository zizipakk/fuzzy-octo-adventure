namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fillisSynced : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("update AspNetUsers set isSynced = 0"));
        }
        
        public override void Down()
        {
        }
    }
}
