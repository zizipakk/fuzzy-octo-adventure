namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statusorderchanging : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //systemparameters
            Sql(string.Format("DELETE StatusToStatus"));
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('502B1C99-3AE3-48B5-80D3-9785DE7E02F9','5DAA7448-7249-4723-BDE1-095959B44C7B','4469A359-9A0B-4F71-A548-02C2924D69BC')")); //V => A
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('26682A6B-8DD2-4EAC-AB48-FD7D5D0E39D6','4469A359-9A0B-4F71-A548-02C2924D69BC','F258AD9F-0637-4478-BEC8-BE5951EBF23D')")); //A => I
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('E93F3175-332A-4A23-B642-C77005C3B50F','F258AD9F-0637-4478-BEC8-BE5951EBF23D','4469A359-9A0B-4F71-A548-02C2924D69BC')")); //I => A
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('49135C58-F1BA-4AE0-95ED-029EE00690AB','4469A359-9A0B-4F71-A548-02C2924D69BC','1AC8CE7E-89BD-481F-941C-A6C93855CD57')")); //A => T
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('22571D89-BD80-4E84-A43B-3681A9C2A45A','1AC8CE7E-89BD-481F-941C-A6C93855CD57','4469A359-9A0B-4F71-A548-02C2924D69BC')")); //T => A
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('090831F5-87D2-4CF2-B436-89A67BA9DA68','F258AD9F-0637-4478-BEC8-BE5951EBF23D','1AC8CE7E-89BD-481F-941C-A6C93855CD57')")); //I => T
        }
        
        public override void Down()
        {
              //nyitás
            Sql(string.Format("--GO"));
            //systemparameters
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('33815623-D8E8-4DE7-886B-12796C807D92','4469A359-9A0B-4F71-A548-02C2924D69BC','5DAA7448-7249-4723-BDE1-095959B44C7B')"));
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('2840879B-77D4-4699-88CA-4DE23713C5EB','F258AD9F-0637-4478-BEC8-BE5951EBF23D','1AC8CE7E-89BD-481F-941C-A6C93855CD57')"));
            Sql(string.Format("INSERT StatusToStatus (Id,FromStatus_Id,ToStatus_Id) VALUES('932BDD39-D217-4E80-B8F8-69DA38192BE5','5DAA7448-7249-4723-BDE1-095959B44C7B','F258AD9F-0637-4478-BEC8-BE5951EBF23D')"));  
        }
    }
}
