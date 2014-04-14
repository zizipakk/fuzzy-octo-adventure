namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CallQueue_Register_Renames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PBXRegistrations", "RegisterTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.PBXRegistrations", "RegiserTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PBXRegistrations", "RegiserTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.PBXRegistrations", "RegisterTime");
        }
    }
}
