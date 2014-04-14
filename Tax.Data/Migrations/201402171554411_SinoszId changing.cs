namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SinoszIdchanging : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SinoszUsers", "SinoszId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SinoszUsers", "SinoszId", c => c.String(nullable: false));
        }
    }
}
