namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeparameters : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //SocialPositions
            Sql(string.Format("UPDATE SocialPositions SET SocialPositionName = 'Országos elnök' WHERE Id = '51FAEF1F-5967-430D-9946-BEB93DB8164A'")); //Elnök
            Sql(string.Format("UPDATE SocialPositions SET SocialPositionName = 'Országos elnökség tagja' WHERE Id = 'FD94D86B-AAD7-4F09-9C3B-E4C24236CDB8'")); //Az elnökség tagja
            //AccountingTypes
            Sql(string.Format("UPDATE AccountingTypes SET Presum = 10000 WHERE Id = 'AD9D875E-B80E-4601-907C-55E40ADD387C'")); //Tagdíj 2014-15 Rendes
        }
        
        public override void Down()
        {
        }
    }
}
