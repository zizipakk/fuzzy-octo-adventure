namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge20140314 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.ScheduleItems", "Description", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.ScheduleItems", "Description");
        }
    }
}
