namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KontaktUsers", "SinoszId", c => c.String());
            DropColumn("dbo.KontaktUsers", "SinuszId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KontaktUsers", "SinuszId", c => c.String());
            DropColumn("dbo.KontaktUsers", "SinoszId");
        }
    }
}
