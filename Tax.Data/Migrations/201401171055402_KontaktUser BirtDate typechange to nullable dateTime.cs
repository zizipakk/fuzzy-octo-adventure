namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KontaktUserBirtDatetypechangetonullabledateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KontaktUsers", "BirthDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KontaktUsers", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
