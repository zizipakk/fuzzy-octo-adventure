namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movingSinoszUserViewModeltoKontaktPortal : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SinoszUsers", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SinoszUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
