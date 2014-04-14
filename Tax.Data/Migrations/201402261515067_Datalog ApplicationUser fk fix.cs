namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatalogApplicationUserfkfix : DbMigration
    {
        public override void Up()
        {
            DropColumn("DataLog.LogRecordChanges", "UserId");
            RenameColumn(table: "DataLog.LogRecordChanges", name: "User_Id", newName: "UserId");
            AlterColumn("DataLog.LogRecordChanges", "UserId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("DataLog.LogRecordChanges", "UserId", c => c.Guid());
            RenameColumn(table: "DataLog.LogRecordChanges", name: "UserId", newName: "User_Id");
            AddColumn("DataLog.LogRecordChanges", "UserId", c => c.Guid());
        }
    }
}
