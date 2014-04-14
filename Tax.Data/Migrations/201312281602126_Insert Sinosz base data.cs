namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertSinoszbasedata : DbMigration
    {
        public override void Up()
        {
            Sql("--GO");
            Sql(string.Format("insert HearingStatus (Id,HearingStatusName) values ('EB00C86A-C87E-49B6-9BAF-4C3ABFC074B6','Halló')"));
            Sql(string.Format("insert HearingStatus (Id,HearingStatusName) values ('92BCF625-97FC-4D91-B620-376F79AD78FA','Siket')"));
            Sql(string.Format("insert HearingStatus (Id,HearingStatusName) values ('981C5B32-5D54-49ED-8DA1-0208C76D5B45','Nagyothalló')"));

            Sql(string.Format("insert MaritalStatus (Id,MaritalStatusName) values ('91DC84B1-CF1F-4E4A-B28C-73A55C317151','Egyedülálló')"));
            Sql(string.Format("insert MaritalStatus (Id,MaritalStatusName) values ('BDA66F37-815B-4EB3-8490-C307B1645DCE','Elvált')"));
            Sql(string.Format("insert MaritalStatus (Id,MaritalStatusName) values ('9DE68A63-31FE-4013-BAF2-0F950CAF8E8A','Özvegy')"));
            Sql(string.Format("insert MaritalStatus (Id,MaritalStatusName) values ('E273E015-02F4-4FB6-908C-CC5D3401E82B','Házas')"));
            Sql(string.Format("insert MaritalStatus (Id,MaritalStatusName) values ('C9301DCC-A70B-4448-A2F8-56D2A12988DF','Élettársi')"));

            Sql(string.Format("insert Educations (Id,EducationName) values ('38930AC8-97B3-4D1F-B461-6BECCEADF084','Kisebb, mint 8 ált.')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('F9CC42F0-A7D3-4718-857B-1467CEAEDEB0','8 általános')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('FBFA5589-DDB6-477D-95F2-96664171F060','Szakiskola')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('2C978E57-9896-455A-8BC9-7EE5D8589799','Szakmunkásképzõ')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('B688EE33-D674-4D74-AEB8-7FC8A9A3A9AD','Szakérettségi')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('726E1D8A-B446-4C06-B3BC-106C2F925EF2','Gimnáziumi érettségi')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('A104E1E9-5A98-486E-B85E-DA021897081F','Okleveles felsõfokú')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('92F75A2F-9EA6-4C28-8A17-85F98B17FC95','Fõiskola')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('4054F783-6170-4143-8328-CCA51879D24B','Egyetem')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('AD7F6DEA-DC02-4660-A2CD-88243EF3A022','Oktatási intézményen kívüli felsõfokú')"));
            Sql(string.Format("insert Educations (Id,EducationName) values ('572C274A-227A-4ABD-A02E-BAFD1B82E180','Oktatási intézményen kívüli középfokú')"));

            Sql(string.Format("insert InjuryTimes (Id,InjuryTimeText) values ('6A09A161-5876-44D4-9656-BCDE5EBEA532','Veleszületett')"));
            Sql(string.Format("insert InjuryTimes (Id,InjuryTimeText) values ('399172A7-2DED-44CE-93E2-6A62993FC2E8','Gyerekkor')"));
            Sql(string.Format("insert InjuryTimes (Id,InjuryTimeText) values ('3CEB7346-F20B-40B8-B972-7F5F45B7374A','Idõskor')"));
            Sql(string.Format("insert InjuryTimes (Id,InjuryTimeText) values ('4D12CCB3-8B6D-4547-AD96-DFDEE56CF352','Felnõttkor')"));

            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('60ABA647-53EA-4911-833F-A63F6B0F2078','SINOSZ Fõvárosi szervezete')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('4DB0A069-961A-4C27-B0ED-4FF19460F243','Bács-Kiskun Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('EA572227-8243-45AA-8FE9-4DC6550D2FC7','Helyi Szervezet Baja')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('DF350BB0-C477-4C63-B520-8CB07F3275A7','Baranya Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('C63EF120-5C21-49F9-9065-82F48B1F0B35','Békés Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('4711898A-7CFF-4286-845B-DAC2768D7C9D','Helyi Szervezet Gyula')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('5B89D2AA-13B6-43BA-9173-685FA9532CFD','Helyi Szervezet Orosháza')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('CCF84619-162E-45D4-851B-E8995976BD3B','Borsod-Abaúj-Zemplén Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('20566A1C-1DCB-40BA-9846-5FD20ED23C67','Helyi Szervezet Kazincbarcika')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('A251C9F5-CFC8-469E-81CE-28C65D376FEF','Dédestapolcsány')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('F142D00A-EA29-4A37-A086-1EFF0964B0D5','Helyi Szervezet Zemplén')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('104C9AD3-1942-4D64-B9A7-1EF841AA3457','Csongrád Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('39BA6AEB-EB1F-47B1-8FA0-41EBB06A73F6','Helyi Szervezet Csongrád - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('13C75A45-0ED7-41EE-9C3C-7CA876501B84','Helyi Szervezet Makó')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('D71B8C47-DDB2-4AFC-A9E2-D9966B6D08EF','Helyi Szervezet Szentes')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('2FD631F6-6EA3-4A41-A5CF-92822B0B89DD','Fejér Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('211FF520-499B-41BD-81A2-3CB221EB2DA7','Helyi Szervezet Dunaújváros')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('592B361B-A42B-44DC-AB3F-CD1D2E2DB5EF','Helyi Szervezet Sopron')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('2869F6CD-39F1-4E96-AB9C-E63699E9A12B','Helyi Szervezet Mosonmagyaróvár')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('DC726F08-ABEB-4822-998B-3ACAD6667F00','SINA-HBME')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('3D3A855E-C6AA-45DE-9C72-B6B4C54223E5','Heves Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('CC3D08FF-8E91-46B3-9CC6-22F39C565711','Helyi Szervezet Gyöngyös')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('C1FF5AF3-D0F1-4288-AA23-43649CDA9B97','SINOSZ Füzesabonyi Klubja - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('0F2D19DF-2977-4687-BD1E-B48CBDB6CC5D','SINOSZ Hatvani Klubja - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('1D2456E6-4809-4ABB-AB63-FF0E5E3F7813','Jász-Nagykun-Szolnok Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('ADCA835C-582A-4C98-B528-A47800AF192C','Komárom-Esztergom Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('2C47E12F-6F4C-4B79-8337-D4E9F1C92F57','Helyi Szervezet Esztergom')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('E4468CBB-F1AC-4A94-9860-2A754C65960D','Nógrád Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('A68E0E4C-D04D-4DF4-9472-1ACD45AD9187','Helyi Szervezet Vác')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('516F381D-0DEF-48CE-BD92-9B35B3247215','Budapesti Siketek Szervezete')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('F6BAB285-DD1A-4DA1-9830-B0A8EC7D7936','Budapesti Nagyothallók Szervezete')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('856730A4-E6FE-4EF2-A871-D55FB14FA25D','Szabolcs-Szatmár-Bereg Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('5CF2B97E-006D-403F-A892-BEC53AE088BB','TOSINA')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('F2E61F91-2E12-48A1-BCB0-E547607CF196','SINOSZ Bonyhádi Klubja - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('53543D22-B9D2-404A-98B0-B363A346598A','SINOSZ Dombovári Klubja - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('99FE2AD4-12C3-4409-B30B-7E95FF5D137F','SINOSZ Nagymányoki Klubja - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('33E02878-C40C-452E-9364-3DBD2E91D8DE','Vas Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('FB253578-DD80-4FEC-ACD2-C364110DD819','Veszprém Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('972D4520-70C0-4349-8AAA-BE433935291C','Hallássérültek Ajkai Egyesülete')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('EC25201A-8CE2-45C7-A49C-42585A76E8C3','Zala Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('C07A5FBB-337E-46F9-BE3C-EB0856262416','Somogy Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('AE2533FB-BEDF-4D3B-99CE-70C88FF591D5','Helyi Szervezet Ózd')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('FD81FF25-2DCA-4A9C-8A3F-A0539B6B9DF4','Gyõr-Moson-Sopron Megyei Szervezet')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('C7CA7C43-1C5D-455D-A145-B52CA9FD4A7F','Helyi Szervezet Hódmezõvásárhely')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('67955554-B943-4C40-858C-A3C5D6B9A984','Helyi Szervezet Solt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('36FD26FB-6D9D-4A7A-A4C5-71582697FA9E','Helyi Szervezet Gyomaendrõd - megszûnt')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('3F543718-791E-49F4-81A1-4859C8FC7BD3','Budapest')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('63F80E02-71E6-4C3E-8243-FA2791943401','Helyi Szervezet Mátészalka')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('8CC30F5D-8079-4A38-BF45-D002015E1D8E','Helyi Szervezet Fehérgyarmat')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('9000E680-DF4D-4489-BE1C-F311B93108DD','Helyi Szervezet Nagykanizsa')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('8C756B86-BFC9-4C34-8FAB-E1DE370943CD','Helyi Szervezet Pápa - megszûnt 2012.12.31.')"));
            Sql(string.Format("insert Organizations (Id,OrganizationName) values ('12BBF800-E771-47A2-817A-829D812F1F34','Helyi Szervezet Érd')"));

            Sql(string.Format("insert Genus (Id,GenusName) values ('7A0696C3-7F4D-4D43-A050-1B631E9CED1E','Férfi')"));
            Sql(string.Format("insert Genus (Id,GenusName) values ('BE403671-A961-4D01-B8B8-E9EF934D472C','Nõ')"));

            Sql(string.Format("insert Nations (Id,NationText) values ('BFB06A45-7C23-4567-BFF5-51C71CBE0D3A','Magyarország')"));

            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('7D2252F2-0FEE-4D29-A400-54F17CBD8C81','Rendes')"));
            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('A78ECE95-11F8-4CF5-9C7F-AA2E700685B3','Pártoló')"));
            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('EB5EBFAA-8E13-4E4B-AE14-977A6D5A22A1','Tanuló')"));
            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('D50D2097-455C-4F50-B63D-8B57FE36E5CA','Tagdíjmentes')"));
            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('3ECE415A-D9AE-41F0-971A-822D11C5C313','Nyugdíjas')"));
            Sql(string.Format("insert Relationships (Id,RelationshipName) values ('3331436D-378B-491A-9C44-9754A134FFB2','Kölyökklub')"));

            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('56C66BA1-AE4B-4001-8F94-83833C9E7682','Tagdíj Rendes')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('B06B7BBA-65CC-46F5-9412-D2E2E62EDA46','Tagsági igazolvány - elsõ, vagy kötelezõ csere')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('6A198E31-F6A1-4878-BA52-3CD6E8EDB12A','Tagdíj Tanuló')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('73750C4D-350F-40C5-B8FA-5C0F517EE138','Tagdíj Kölyökklub')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('616115B1-E246-40D8-B8B0-028ABAB14ED7','Tagsági igazolvány - csere igazolatlan okból')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('5C536BF2-E850-48A6-BA7E-7F16514DC4F4','Tagsági igazolvány - csere igazolt okból')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('0F3BB22D-001C-4C30-91EF-6777B42B3C83','Tagdíj Nyugdíjas')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('BCB75DDB-EE6C-41B5-A92F-3D7B9BCDACA0','Visszafizetés')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('01C19024-F488-468D-B39B-6C95DE41C593','Elnöki Tagdíjmentes')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('CF81EF61-CAA7-4891-8469-2B36BC053488','Tiszteletbeli Tag Tagdíjmentes')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('AD9D875E-B80E-4601-907C-55E40ADD387C','Tagdíj 2014-15 Rendes')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('655CF7D2-4AA9-414A-992B-A7C80C75E312','Tagdíj 2014-15 65 év felett')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('5894D393-320A-4EBC-B10E-C9DC304FB726','Tagdíj 0-6 éves korig')"));
            Sql(string.Format("insert AccountingTypes (Id,AccountingTypeName) values ('779F6E7C-C0FA-4715-B2AD-26AD0FE3D152','Tagdíj 2014-15 Tanuló')"));

            Sql(string.Format("insert AccountingStatus (Id,AccountingStatusName) values ('22BABFE2-DE43-4571-B66D-86E57F91CA98','Tény')"));
            Sql(string.Format("insert AccountingStatus (Id,AccountingStatusName) values ('4FD530EB-8626-4AE0-9CA0-191C40F45607','Törölve')"));

            Sql(string.Format("insert CardStatus (Id,CardStatusName) values ('11B7E71D-7C1C-48DB-B664-51059E7EACB7','Regisztrálva')"));
            Sql(string.Format("insert CardStatus (Id,CardStatusName) values ('F47B444A-F5EE-4CF0-80BB-F68397FB98D8','Küldve')"));
            Sql(string.Format("insert CardStatus (Id,CardStatusName) values ('AAF9813E-EF0A-4C1C-BD23-D532B6167C79','Visszaérkezett')"));
            Sql(string.Format("insert CardStatus (Id,CardStatusName) values ('ABE79C45-EF02-4AD5-87B5-AE56CC737278','Átadott')"));

            Sql(string.Format("insert NewsTypes (Id,NewsTypeName) values ('885D805C-5B18-4C80-BF6D-9F2E5219BCCE','Üdvözlet')"));
            Sql(string.Format("insert NewsTypes (Id,NewsTypeName) values ('4380B28D-B861-4ECA-B5C5-6E19F4A31298','Hír')"));

            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('C89F6E06-85E2-43E5-8823-0565AB526D2E','Önéletrajz')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('D2FE4547-87AD-4577-97C8-EC4847DE481E','Audiogram 2. oldal')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('FD1B64ED-9DE5-48A9-A10A-81F71E2098D3','Audiogram')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('84B3AF1C-08B4-40BA-8C6F-34F3E53EF907','FNY 41-2 Átigazolási kérelem')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('F1D23085-E073-42F2-B801-80FAC7F5A266','Rongálódott igazolvány')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('B4DACB95-DD0C-40E8-959B-84400E4CC94B','FNY 41-1 Adatváltozás bejelentése')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('40B83F5B-A097-4F65-AF2A-25C813F7D23A','Rendõrségi jkv igazolványcseréhez')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('564D8369-FD37-4EC9-B9D5-3E15EAAF75AF','Diákig. vagy iskolalátogatás')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('AB815820-C609-4707-B4CE-3B1D5DBDAB34','Tanulói nyilatkozat')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('4529C1C1-8F60-43A5-AA9B-2D248FE07F2A','Lakhelyigazolás külföldi állampolgárnak')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('B2231C4F-8E49-43BC-97B3-F57E98619DDB','Egyéb igazolás, melléklet')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('768DB0C6-22FD-43E0-B5F9-9BE5317931DB','Belépési nyilatkozat')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('9E2EE06F-2E44-40C2-AE7F-F3A149DCD0A5','Elnöki tagdíjmentes nyilatkozat')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('84C51A1C-440C-4999-9BF0-3993C1D8C045','Értesítés ellenõrzõ vizsg')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('91A7A8D1-8406-4B2A-9FAD-F5128C344226','Ig. csere igazolás 2')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('B1697EFA-A2BB-4FC3-AE16-1FF7431C7A88','Magasabb ö. csp. igazolása')"));
            Sql(string.Format("insert FileTypes (Id,FileTypeName) values ('12A58621-47A1-4C7D-BEEE-20DBDC632AD2','Értesítés elutasításról')"));

            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('1296C202-3ABF-4B67-8E75-0453D64DC495','Otthoni')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('19F612CB-FF48-462C-9376-421C07C4EF0D','Mobil')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('26F9CF3E-23D9-4BAA-93EC-F2444AC4E361','Munkahely')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('9EFAAA4E-6114-4BD3-9726-461DE9697B49','Fax')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('99CBE12F-89C3-48B6-9976-CB478C60AE44','E-mail')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('DC685091-1F28-4B5C-9CEA-838B161258FE','Egyéb lakcím')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('842EDF60-9C60-4D20-B8F9-73BFAEEE3C2D','E-mail 2')"));
            Sql(string.Format("insert AddressTypes (Id,AddressTypeName) values ('77D9A750-DE6A-4033-96AF-0D08B03222ED','Kapcs.tartó telszám')"));

            Sql(string.Format("insert SinoszUserStatus (Id,StatusName) values ('4469A359-9A0B-4F71-A548-02C2924D69BC','Aktív')"));
            Sql(string.Format("insert SinoszUserStatus (Id,StatusName) values ('F258AD9F-0637-4478-BEC8-BE5951EBF23D','Inaktív')"));
            Sql(string.Format("insert SinoszUserStatus (Id,StatusName) values ('1AC8CE7E-89BD-481F-941C-A6C93855CD57','Törölt')"));
            Sql(string.Format("insert SinoszUserStatus (Id,StatusName) values ('5DAA7448-7249-4723-BDE1-095959B44C7B','Várakozó')"));

            Sql(string.Format("insert StatusToStatus (Id,FromStatus_Id,ToStatus_Id) values ('33815623-D8E8-4DE7-886B-12796C807D92','4469A359-9A0B-4F71-A548-02C2924D69BC','5DAA7448-7249-4723-BDE1-095959B44C7B')"));
            Sql(string.Format("insert StatusToStatus (Id,FromStatus_Id,ToStatus_Id) values ('932BDD39-D217-4E80-B8F8-69DA38192BE5','5DAA7448-7249-4723-BDE1-095959B44C7B','F258AD9F-0637-4478-BEC8-BE5951EBF23D')"));
            Sql(string.Format("insert StatusToStatus (Id,FromStatus_Id,ToStatus_Id) values ('2840879B-77D4-4699-88CA-4DE23713C5EB','F258AD9F-0637-4478-BEC8-BE5951EBF23D','1AC8CE7E-89BD-481F-941C-A6C93855CD57')"));
        }
        
        public override void Down()
        {
        }
    }
}
