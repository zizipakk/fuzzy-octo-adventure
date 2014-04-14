namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EngagetheSinoszUserssearch2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SinoszUsers", "LastAccountingDocument", c => c.Guid());
            AlterColumn("dbo.SinoszUsers", "LastCard", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SinoszUsers", "LastCard", c => c.Guid(nullable: false));
            AlterColumn("dbo.SinoszUsers", "LastAccountingDocument", c => c.Guid(nullable: false));
        }
    }
}
