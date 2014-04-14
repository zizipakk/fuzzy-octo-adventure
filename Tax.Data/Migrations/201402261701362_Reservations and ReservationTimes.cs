namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservationsandReservationTimes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        isPresent = c.Boolean(nullable: false),
                        KontaktUser_Id = c.Guid(),
                        ReservationTime_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.KontaktUsers", t => t.KontaktUser_Id)
                .ForeignKey("dbo.ReservationTimes", t => t.ReservationTime_Id)
                .Index(t => t.KontaktUser_Id)
                .Index(t => t.ReservationTime_Id);
            
            CreateTable(
                "dbo.ReservationTimes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReservationDate = c.DateTime(nullable: false),
                        ReservationMax = c.Int(nullable: false),
                        ReservationCurrent = c.Int(nullable: false),
                        ReservationBegin = c.DateTime(nullable: false),
                        isEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "ReservationTime_Id", "dbo.ReservationTimes");
            DropForeignKey("dbo.Reservations", "KontaktUser_Id", "dbo.KontaktUsers");
            DropIndex("dbo.Reservations", new[] { "ReservationTime_Id" });
            DropIndex("dbo.Reservations", new[] { "KontaktUser_Id" });
            DropTable("dbo.ReservationTimes");
            DropTable("dbo.Reservations");
        }
    }
}
