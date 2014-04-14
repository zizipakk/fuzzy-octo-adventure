namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pumptesztdataintosinoszuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SinoszUsers", "SinoszId", c => c.String());
            DropColumn("dbo.SinoszUsers", "SinuszId");

            Sql("--GO");
            Sql(string.Format("insert SinoszUsers (Id,SinoszId,BirthDate) values ('D1B81D0E-E618-4031-8DF4-F9345199AE6C','123','2012/01/01')"));
        }
        
        public override void Down()
        {
            AddColumn("dbo.SinoszUsers", "SinuszId", c => c.String());
            DropColumn("dbo.SinoszUsers", "SinoszId");
        }
    }
}
