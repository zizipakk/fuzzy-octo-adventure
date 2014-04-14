namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EngagetheSinoszUserssearch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SinoszUsers", "LastAccountingDocument", c => c.Guid(nullable: false));
            AddColumn("dbo.SinoszUsers", "LastCard", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SinoszUsers", "LastCard");
            DropColumn("dbo.SinoszUsers", "LastAccountingDocument");
        }
    }
}
