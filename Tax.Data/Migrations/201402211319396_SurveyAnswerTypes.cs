namespace Tax.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyAnswerTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SurveyAnswerCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Survey = c.String(nullable: false),
                        QuestionCode = c.String(nullable: false),
                        AnswerCode = c.String(nullable: false),
                        AnswerText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyQuestionCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Survey = c.String(nullable: false),
                        QuestionCode = c.String(nullable: false),
                        QuestionText = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            //Kérdõív kérdések és válaszok
            Sql(string.Format("--GO"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','1',N'Hogyan értékeled a szolgáltatáshoz való kapcsolódást?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','2',N'Mennyire voltál elégedett az akadálymentesítéssel?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','3',N'Ha nem felelt meg, annak mi volt az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','4',N'Mennyire vagy elégedett összességében a Kontakt tolmácsszolgáltatással?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','5',N'Hogyan jellemeznéd a Kontakt tolmácsszolgáltatást? (Kérjük, hogy maximum 3 választ jelölj meg!)')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','1',N'Hogyan értékeled a szolgáltatáshoz való kapcsolódást?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','2',N'Mennyire voltál elégedett az akadálymentesítéssel?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','3',N'Ha nem felelt meg, annak mi volt az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','4',N'Mennyire jól látható a tolmács és a jelelés (mimika láthatósága, kézmozgások láthatósága, jelelési tér nagysága… stb.)?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','5',N'Mennyire vagy elégedett összességében a Kontakt tolmácsszolgáltatással?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','6',N'Hogyan jellemeznéd a Kontakt tolmácsszolgáltatást? (Kérjük, hogy maximum 3 választ jelölj meg!)')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'HE','1',N'Hívástípus-kategóriák')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','1',N'Hogyan értékeled a szolgáltatáshoz való kapcsolódást?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','2',N'Melyik típusú akadálymentesítési módot vette igénybe?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','3',N'Hogyan értékeli az akadálymentesítés megvalósítását?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','4',N'Ha nem felelt meg, annak mi volt az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','5',N'Mennyire elégedett összességében a Kontakt tolmácsszolgáltatással?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','1',N'Mennyire értetted a halló ügyfelet tolmácsolás közben (tapasztalható-e szakadozás, tisztán érthetõ-e a hang…stb.)?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','2',N'Ha nehezen értetted a halló ügyfelet, annak mi az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','3',N'Mennyire volt követhetõ számodra a hallássérült ügyfél?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','4',N'Mennyire látható jól a hallássérült ügyfél a tolmácsolás ideje alatt (mimika láthatósága, kézmozgások láthatósága, szakadozik-e a kép… stb.)?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','5',N'Melyik akadálymentesítési módot részesíted elõnyben?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','6',N'Mennyire vagy elégedett összességében a Kontakt tolmácsszolgáltatással?')"));

            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','1','a',N'azonnal sikerült csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','1','b',N'többszöri próbálkozás után sikerült csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','a',N'kiváló minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','b',N'jó minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','c',N'közepes minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','d',N'megfelelõ minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','e',N'nem felelt meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','a',N'különbözõ jelhasználat')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','b',N'fordítás gyorsasága')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','c',N'ismeretlen számomra a tolmács')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','d',N'számomra nem érthetõ, új jelek')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','e',N'technikai okok (nem látom rendesen a tolmácsot, szakadozik a kép, kicsi a tolmácsablak, rossz a megvilágítás)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','a',N'kiválóan látható, követhetõ')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','b',N'jól látható, követhetõ')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','c',N'részben jól látható')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','d',N'rosszul látható, de még követhetõ')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','e',N'nagyon rosszul látható, követhetetlen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','a',N'nagyon elégedett vagyok, szívesen használnám rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','b',N'elégedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','c',N'elfogadható, de még szokatlan')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','d',N'nem vagyok elégedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','a',N'korszerû akadálymentesítési mód')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','b',N'kényelmes megoldás számomra')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','c',N'idõt takarítok meg vele')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','d',N'jó, hogy nem kell utaznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','e',N'elõnyös, hogy bizalmas helyzetekben nincs jelen a tolmács (pl. orvosnál)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','f',N'elõnyös, hogy bárhonnan elérhetõ a szolgáltatás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','1','a',N'azonnal sikerült csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','1','b',N'többszöri próbálkozás után sikerült csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','2','a',N'jelnyelvi tolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','2','b',N'írótolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','a',N'kiváló minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','b',N'jó minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','c',N'közepes minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','d',N'megfelelõ minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','e',N'nem felelt meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','a',N'a jelnyelvi/írótolmácsolás tempója')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','b',N'a tolmácsolás akadozása')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','c',N'a tolmácsolás pontatlansága')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','d',N'technikai akadályok (szakadozás tapasztalható, nem érthetõ tisztán a hang, nem olvasható megfelelõen a szöveg…stb.)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','a',N'nagyon elégedett vagyok, szívesen használnám rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','b',N'elégedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','c',N'elfogadható, de még szokatlan')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','d',N'nem vagyok elégedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','a',N'Közhatalmat gyakorló szerv eljárása során végzett tolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','b',N'Egészségügyi ellátás igénybevételekor végzett tolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','c',N'Foglalkoztatási célú és munkahelyen igénybe vett tolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','d',N'A jogosult választott tisztségének ellátásához igénybe vett tolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','e',N'A jogosult önálló életvitelének elõsegítése céljából végzett tolmácsolás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','f',N'Sürgõsségi távtolmácsolás (segélyhívás)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','1','a',N'azonnal sikerült csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','1','b',N'többszöri próbálkozás után sikerült csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','a',N'kiváló minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','b',N'jó minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','c',N'közepes minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','d',N'megfelelõ minõségû volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','e',N'nem felelt meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','a',N'a felhasználói felület nem kényelmes (képernyõ, betûméret, betûstílus, színek…stb.)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','b',N'a megjelenõ szöveg nem folyamatos')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','c',N'a tolmácsolás tempója')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','d',N'sok az elütés')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','a',N'nagyon elégedett vagyok, szívesen használnám rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','b',N'elégedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','c',N'elfogadható, de még szokatlan')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','d',N'nem vagyok elégedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','a',N'korszerû akadálymentesítési mód')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','b',N'kényelmes megoldás számomra')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','c',N'idõt takarítok meg vele')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','d',N'jó, hogy nem kell utaznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','e',N'elõnyös, hogy bizalmas helyzetekben nincs jelen a tolmács (pl. orvosnál)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','f ',N'elõnyös, hogy bárhonnan elérhetõ a szolgáltatás')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','a',N'kiválóan értettem mindent')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','b',N'jól értettem')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','c',N'nem értettem mindent')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','d',N'keveset értettem meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','e',N'semmit nem értettem')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','a',N'a beszédtempó gyorsasága')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','b',N'beszédtempó lassúsága')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','c',N'mûszaki ok (szakadozik a vonal, nem érthetõ a halló ügyfél beszéde…stb.)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','d',N'túl sok idegen kifejezés')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','a',N'kiválóan értettem mindent, nagyon jól követhetõ volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','b',N'jól értettem, követhetõ volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','c',N'nem értettem mindent, részben jól követhetõ volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','d',N'keveset értettem meg, de még követhetõ volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','e',N'semmit nem értettem, követhetetlen volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','a',N'kiválóan, pontosan látható')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','b',N'jól látható')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','c',N'nem látható jól')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','d',N'rosszul látható')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','e',N'nagyon rosszul látható')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','a',N'Az írótolmácsolást')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','b',N'A jelnyelvi tolmácsolást')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','c',N'mindkettõt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','d',N'mindegy')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','a',N'nagyon elégedett vagyok, szívesen használnám rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','b',N'elégedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','c',N'közepesen vagyok elégedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','d',N'nem vagyok elégedett')"));

        }
        
        public override void Down()
        {
            DropTable("dbo.SurveyQuestionCodes");
            DropTable("dbo.SurveyAnswerCodes");
        }
    }
}
