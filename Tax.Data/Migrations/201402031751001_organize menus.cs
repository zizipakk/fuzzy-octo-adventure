namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class organizemenus : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("delete Menus where Id = '44d6d88d-d2ef-4307-8a4e-88a42937d64f'"));
            Sql(string.Format("update Menus set Controller = '', Action = ''"));            
        }
        
        public override void Down()
        {
        }
    }
}
