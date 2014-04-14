namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mustfillthediscriminator : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("update SinoszUsers set Discriminator = 'SinoszUser'"));
        }
        
        public override void Down()
        {
        }
    }
}
