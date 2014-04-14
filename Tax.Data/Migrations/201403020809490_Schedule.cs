namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClosedSchedules",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SchedulePeriodStart = c.DateTime(nullable: false),
                        SchedulePeriodEnd = c.DateTime(nullable: false),
                        ClosureTimestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduleItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Activity = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);

            Sql(string.Format("--GO"));

            Sql(string.Format("INSERT INTO [dbo].[SubMenus] (Id, Name, Controller, Action, isActive, Position, Menu_Id) VALUES ('CD6FE1FA-4443-46C2-AFFB-63E42B8FF791', 'Beosztás készítés', 'Schedule', 'Edit', 1, 1, '1F2F9271-D810-478E-9AE7-0BE5667A9127')"));
            Sql(string.Format("INSERT INTO [dbo].[SubMenus] (Id, Name, Controller, Action, isActive, Position, Menu_Id) VALUES ('65E2A398-5BE4-4262-9F90-EFD6481A8F39', 'Beosztás', 'Schedule', 'Display', 1, 2, '1F2F9271-D810-478E-9AE7-0BE5667A9127')"));

            Sql(string.Format("INSERT INTO [dbo].SubMenuKontaktRoles (KontaktRole_Id, SubMenu_Id) VALUES ('af85bf54-7f32-4fae-b003-3ff9d3fea603', 'CD6FE1FA-4443-46C2-AFFB-63E42B8FF791')"));
            Sql(string.Format("INSERT INTO [dbo].SubMenuKontaktRoles (KontaktRole_Id, SubMenu_Id) VALUES ('bf07d7ef-a40f-488e-9e82-69eb2357cd11', '65E2A398-5BE4-4262-9F90-EFD6481A8F39')"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleItems", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ScheduleItems", new[] { "User_Id" });
            DropTable("dbo.ScheduleItems");
            DropTable("dbo.ClosedSchedules");

            Sql(string.Format("--GO"));

            Sql(string.Format("delete SubMenuKontaktRoles where SubMenu_Id = 'CD6FE1FA-4443-46C2-AFFB-63E42B8FF791'"));
            Sql(string.Format("delete SubMenuKontaktRoles where SubMenu_Id = '65E2A398-5BE4-4262-9F90-EFD6481A8F39'"));

            Sql(string.Format("delete SubMenus where Id = 'CD6FE1FA-4443-46C2-AFFB-63E42B8FF791'"));
            Sql(string.Format("delete SubMenus where Id = '65E2A398-5BE4-4262-9F90-EFD6481A8F39'"));
        }
    }
}
