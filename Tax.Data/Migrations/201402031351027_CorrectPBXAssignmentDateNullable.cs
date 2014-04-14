namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectPBXAssignmentDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PBXExtensionDatas", "StartTime", c => c.DateTime());
            AlterColumn("dbo.PBXExtensionDatas", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PBXExtensionDatas", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PBXExtensionDatas", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
