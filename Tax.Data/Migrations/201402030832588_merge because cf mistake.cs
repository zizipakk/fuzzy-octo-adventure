namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mergebecausecfmistake : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.SinoszUsers", "LastAccountingDocument", c => c.Guid());
            //AddColumn("dbo.SinoszUsers", "LastCard", c => c.Guid());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.SinoszUsers", "LastCard");
            //DropColumn("dbo.SinoszUsers", "LastAccountingDocument");
        }
    }
}
