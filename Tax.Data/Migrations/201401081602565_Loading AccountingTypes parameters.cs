namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoadingAccountingTypesparameters : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=4000,isMembershipCost=0,isCardCost=1,isEnabled=1,Relationship_Id='5AFCAD8E-1B14-4F45-81DC-48396F6449C8' from AccountingTypes where Id = '616115B1-E246-40D8-B8B0-028ABAB14ED7'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=5000,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='EB5EBFAA-8E13-4E4B-AE14-977A6D5A22A1' from AccountingTypes where Id = '779F6E7C-C0FA-4715-B2AD-26AD0FE3D152'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=0,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='D50D2097-455C-4F50-B63D-8B57FE36E5CA' from AccountingTypes where Id = 'CF81EF61-CAA7-4891-8469-2B36BC053488'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=3000,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='EB5EBFAA-8E13-4E4B-AE14-977A6D5A22A1' from AccountingTypes where Id = '6A198E31-F6A1-4878-BA52-3CD6E8EDB12A'"));
            Sql(string.Format("update AccountingTypes set isFixsum=0,Presum=0,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='5AFCAD8E-1B14-4F45-81DC-48396F6449C8' from AccountingTypes where Id = 'BCB75DDB-EE6C-41B5-A92F-3D7B9BCDACA0'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=1000,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='7D2252F2-0FEE-4D29-A400-54F17CBD8C81' from AccountingTypes where Id = 'AD9D875E-B80E-4601-907C-55E40ADD387C'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=1250,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='3331436D-378B-491A-9C44-9754A134FFB2' from AccountingTypes where Id = '73750C4D-350F-40C5-B8FA-5C0F517EE138'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=3000,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='3ECE415A-D9AE-41F0-971A-822D11C5C313' from AccountingTypes where Id = '0F3BB22D-001C-4C30-91EF-6777B42B3C83'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=0,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='D50D2097-455C-4F50-B63D-8B57FE36E5CA' from AccountingTypes where Id = '01C19024-F488-468D-B39B-6C95DE41C593'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=1200,isMembershipCost=0,isCardCost=1,isEnabled=1,Relationship_Id='5AFCAD8E-1B14-4F45-81DC-48396F6449C8' from AccountingTypes where Id = '5C536BF2-E850-48A6-BA7E-7F16514DC4F4'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=6000,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='7D2252F2-0FEE-4D29-A400-54F17CBD8C81' from AccountingTypes where Id = '56C66BA1-AE4B-4001-8F94-83833C9E7682'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=5000,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='3ECE415A-D9AE-41F0-971A-822D11C5C313' from AccountingTypes where Id = '655CF7D2-4AA9-414A-992B-A7C80C75E312'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=0,isMembershipCost=1,isCardCost=0,isEnabled=1,Relationship_Id='3331436D-378B-491A-9C44-9754A134FFB2' from AccountingTypes where Id = '5894D393-320A-4EBC-B10E-C9DC304FB726'"));
            Sql(string.Format("update AccountingTypes set isFixsum=1,Presum=800,isMembershipCost=0,isCardCost=1,isEnabled=1,Relationship_Id='5AFCAD8E-1B14-4F45-81DC-48396F6449C8' from AccountingTypes where Id = 'B06B7BBA-65CC-46F5-9412-D2E2E62EDA46'"));
        }
        
        public override void Down()
        {
        }
    }
}
