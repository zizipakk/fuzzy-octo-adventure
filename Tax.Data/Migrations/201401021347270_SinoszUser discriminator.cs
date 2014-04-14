namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SinoszUserdiscriminator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SinoszUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));

            Sql("--GO");
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('B58A8E39-1E18-447D-9950-1E95BECBC697','Nincs')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('986CD8BD-405C-4A5E-A7A4-56F91455AE81','Öregségi')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('FC5C3C33-9ABD-4EFB-BECE-72C00B80B58C','Özvegyi')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('25389A2B-291A-438F-B597-58D73C4C4B71','Rokk. III. fok')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('828E945D-8096-44C3-A5E1-B76D6A3F14E7','Árvaellátás')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('298512D8-0D65-49A5-88D3-5399ED61AC96','Rokk. járadék')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('9A77869A-889E-4FC9-9B20-9B6C9C05B5E2','Rehab. járadék')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('5096791D-6C40-41BB-9E02-BFD9539DD628','Rokk. I. fok')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('CC05CDB9-CE25-4974-8DAC-19362858A69E','Rokk. II. fok')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('DFF9BAFC-863C-428F-8D20-1592A82EFFBC','Rehab. ellátás')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('32703895-8875-421E-A310-EF332E6EE05F','Rokk. ellátás')"));
            Sql(string.Format("insert PensionTypes (Id,PensionTypeName) values ('6CFA1D23-5E4E-4B0F-962B-A04F6CAC2863','Kork. nyugdíj')"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SinoszUsers", "Discriminator");
        }
    }
}
