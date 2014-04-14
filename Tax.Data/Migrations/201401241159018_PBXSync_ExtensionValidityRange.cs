namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PBXSync_ExtensionValidityRange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PBXExtensionDatas", "isSynced", c => c.Boolean(nullable: false));
            AddColumn("dbo.PBXExtensionDatas", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.PBXExtensionDatas", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "isSynced", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "isSynced");
            DropColumn("dbo.PBXExtensionDatas", "EndTime");
            DropColumn("dbo.PBXExtensionDatas", "StartTime");
            DropColumn("dbo.PBXExtensionDatas", "isSynced");
        }
    }
}
