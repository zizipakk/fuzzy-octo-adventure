namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParcelNumbertoInterpreterCenter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterpreterCenters", "ParcelNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterpreterCenters", "ParcelNumber");
        }
    }
}
