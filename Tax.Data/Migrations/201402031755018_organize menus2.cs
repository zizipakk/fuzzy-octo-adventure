namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class organizemenus2 : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("update Menus set Position = 5 where Id = 'c66a7876-06d1-40db-9a12-48627ee05708'"));   
        }
        
        public override void Down()
        {
        }
    }
}
