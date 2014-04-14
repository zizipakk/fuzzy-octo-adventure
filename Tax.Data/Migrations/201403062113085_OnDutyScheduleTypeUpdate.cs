namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnDutyScheduleTypeUpdate : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("--GO"));

            Sql(string.Format("UPDATE ScheduleItems SET Activity = 0"));
        }
        
        public override void Down()
        {
        }
    }
}
