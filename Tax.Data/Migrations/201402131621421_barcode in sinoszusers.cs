namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class barcodeinsinoszusers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SinoszUsers", "Barcode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SinoszUsers", "Barcode");
        }
    }
}
