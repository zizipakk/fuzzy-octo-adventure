namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneNumberschanging : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Files", "file_type", c => c.String());
            DropColumn("dbo.PBXExtensionDatas", "ExtensionID");
            DropColumn("dbo.PBXExtensionDatas", "Password");

            Sql("--GO");
            Sql(string.Format("update dbo.PhoneNumbers set ExternalPhoneNumber = InnerPhoneNumber from dbo.PhoneNumbers where ExternalPhoneNumber = ''"));
        }
        
        public override void Down()
        {
            AddColumn("dbo.PBXExtensionDatas", "Password", c => c.String());
            AddColumn("dbo.PBXExtensionDatas", "ExtensionID", c => c.String());
            //AlterColumn("dbo.Files", "file_type", c => c.String());
        }
    }
}
