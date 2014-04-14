namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedatetimetotimespaninreservationtimesreservationbegin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReservationTimes", "ReservationBegin", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReservationTimes", "ReservationBegin", c => c.DateTime(nullable: false));
        }
    }
}
