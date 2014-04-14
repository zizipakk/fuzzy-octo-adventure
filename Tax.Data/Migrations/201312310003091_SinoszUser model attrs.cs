namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SinoszUsermodelattrs : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("update SinoszUsers set SinoszUserName='Teszt Tag',BirthPlace='Teszt V�ros',MothersName='Teszt Anya',HomeAddress='Teszt Lakc�m',DecreeNumber='Teszt Hat�rozat' where Id='d1b81d0e-e618-4031-8df4-f9345199ae6c'"));

            AlterColumn("dbo.SinoszUsers", "SinoszId", c => c.String(nullable: false));
            AlterColumn("dbo.SinoszUsers", "SinoszUserName", c => c.String(nullable: false));
            AlterColumn("dbo.SinoszUsers", "BirthPlace", c => c.String(nullable: false));
            AlterColumn("dbo.SinoszUsers", "MothersName", c => c.String(nullable: false));
            AlterColumn("dbo.SinoszUsers", "HomeAddress", c => c.String(nullable: false));
            AlterColumn("dbo.SinoszUsers", "DecreeNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SinoszUsers", "DecreeNumber", c => c.String());
            AlterColumn("dbo.SinoszUsers", "HomeAddress", c => c.String());
            AlterColumn("dbo.SinoszUsers", "MothersName", c => c.String());
            AlterColumn("dbo.SinoszUsers", "BirthPlace", c => c.String());
            AlterColumn("dbo.SinoszUsers", "SinoszUserName", c => c.String());
            AlterColumn("dbo.SinoszUsers", "SinoszId", c => c.String());
        }
    }
}
