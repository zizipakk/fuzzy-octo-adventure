namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pumpSocialPositions : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //SocialPositions
            Sql(string.Format("INSERT SocialPositions (Id,SocialPositionName) VALUES('51FAEF1F-5967-430D-9946-BEB93DB8164A','Elnök')"));
            Sql(string.Format("INSERT SocialPositions (Id,SocialPositionName) VALUES('463658B1-95F2-43C0-84B6-3FC8FE343A8A','Alelnök')"));
            Sql(string.Format("INSERT SocialPositions (Id,SocialPositionName) VALUES('FD94D86B-AAD7-4F09-9C3B-E4C24236CDB8','Az elnökség tagja')"));
            Sql(string.Format("INSERT SocialPositions (Id,SocialPositionName) VALUES('0D22D689-CE6D-4BD1-919F-32D8156A0496','A Felügyelõ Bizottság és a Fegyelmi Bizottság tagja')"));
            Sql(string.Format("INSERT SocialPositions (Id,SocialPositionName) VALUES('4C26B2B2-6E51-4EE4-B2B5-ED8DE20519DF','megyei/helyi elnökök és elnökségi tagok')"));
        }
        
        public override void Down()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //SocialPositions
            Sql(string.Format("DELETE SocialPositions WHERE Id = '51FAEF1F-5967-430D-9946-BEB93DB8164A'"));
            Sql(string.Format("DELETE SocialPositions WHERE Id = '463658B1-95F2-43C0-84B6-3FC8FE343A8A'"));
            Sql(string.Format("DELETE SocialPositions WHERE Id = 'FD94D86B-AAD7-4F09-9C3B-E4C24236CDB8'"));
            Sql(string.Format("DELETE SocialPositions WHERE Id = '0D22D689-CE6D-4BD1-919F-32D8156A0496'"));
            Sql(string.Format("DELETE SocialPositions WHERE Id = '4C26B2B2-6E51-4EE4-B2B5-ED8DE20519DF'"));
        }
    }
}
