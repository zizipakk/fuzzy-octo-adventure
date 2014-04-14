namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pumplimitsinareas : DbMigration
    {
        public override void Up()
        {
            //nyitás
            Sql(string.Format("--GO"));
            //areas
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = '2f453cdf-ed6d-403a-91b8-1205e454c17e'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = '0d5a5e1a-07cd-4e41-b7d6-1daad89f7bb6'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '52' where Id = '0786b3c6-358a-4233-b156-4ea3eb175843'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = '7f869778-02aa-4650-af4d-a192e63eb405'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = 'a5a1464d-c8df-4303-9841-a51e26c705d7'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = 'e3b32ba0-97e4-466a-8213-afd735846d0d'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = '1ec4df66-7f7d-4b65-9f58-b7e638b5a885'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '26' where Id = '8529d07a-2cda-4162-8431-c23c91151f82'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '26' where Id = '34125765-f435-409f-a8a2-c7d25f4e0b57'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '52' where Id = 'a3abb4d7-3a93-4e98-bd01-dc370e3ef616'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '100', DeviceNumberLimit = '39' where Id = 'cbff9936-20b6-41cb-927f-fa3fe9655f98'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '200', DeviceNumberLimit = '65' where Id = 'cea43d78-f6ef-4f3a-a7de-4b750f0d040c'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '200', DeviceNumberLimit = '52' where Id = 'ce4f9338-ae9c-4274-915f-722385313dd9'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '200', DeviceNumberLimit = '65' where Id = 'cdc9ab2d-5d7b-4e5f-af0f-7d20276aa51f'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '200', DeviceNumberLimit = '52' where Id = '43160955-d31a-434b-acf9-a5bd15af1112'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '200', DeviceNumberLimit = '78' where Id = '78495b71-351f-40f9-b7c4-fa355cff6597'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '300', DeviceNumberLimit = '91' where Id = '35599bdd-ba7a-464e-86a9-5073b36a0bde'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '300', DeviceNumberLimit = '91' where Id = '77ce56ec-f2cc-4201-be32-f31861885e35'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '700', DeviceNumberLimit = '117' where Id = '59ac6f70-ccc3-41aa-b1c5-c2c66d030110'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '1300', DeviceNumberLimit = '260' where Id = '2afdd97f-d2ff-4568-8402-cf7c95c0ed31'"));
            Sql(string.Format("update Areas set PhoneNumberLimit = '0', DeviceNumberLimit = '20' where Id = '266dccb5-9f39-4b1c-a854-d057c7daa305'"));

        }
        
        public override void Down()
        {
        }
    }
}
