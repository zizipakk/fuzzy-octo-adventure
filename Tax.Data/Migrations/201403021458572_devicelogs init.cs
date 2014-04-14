namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class devicelogsinit : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));

            Sql(string.Format("update DeviceLogs set Area_Id = '266DCCB5-9F39-4B1C-A854-D057C7DAA305'	from DeviceLogs"));
        }
        
        public override void Down()
        {
        }
    }
}
