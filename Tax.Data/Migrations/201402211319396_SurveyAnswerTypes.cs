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

            //K�rd��v k�rd�sek �s v�laszok
            Sql(string.Format("--GO"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','1',N'Hogyan �rt�keled a szolg�ltat�shoz val� kapcsol�d�st?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','2',N'Mennyire volt�l el�gedett az akad�lymentes�t�ssel?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','3',N'Ha nem felelt meg, annak mi volt az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','4',N'Mennyire vagy el�gedett �sszess�g�ben a Kontakt tolm�csszolg�ltat�ssal?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'I','5',N'Hogyan jellemezn�d a Kontakt tolm�csszolg�ltat�st? (K�rj�k, hogy maximum 3 v�laszt jel�lj meg!)')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','1',N'Hogyan �rt�keled a szolg�ltat�shoz val� kapcsol�d�st?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','2',N'Mennyire volt�l el�gedett az akad�lymentes�t�ssel?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','3',N'Ha nem felelt meg, annak mi volt az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','4',N'Mennyire j�l l�that� a tolm�cs �s a jelel�s (mimika l�that�s�ga, k�zmozg�sok l�that�s�ga, jelel�si t�r nagys�ga� stb.)?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','5',N'Mennyire vagy el�gedett �sszess�g�ben a Kontakt tolm�csszolg�ltat�ssal?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'U','6',N'Hogyan jellemezn�d a Kontakt tolm�csszolg�ltat�st? (K�rj�k, hogy maximum 3 v�laszt jel�lj meg!)')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'HE','1',N'H�v�st�pus-kateg�ri�k')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','1',N'Hogyan �rt�keled a szolg�ltat�shoz val� kapcsol�d�st?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','2',N'Melyik t�pus� akad�lymentes�t�si m�dot vette ig�nybe?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','3',N'Hogyan �rt�keli az akad�lymentes�t�s megval�s�t�s�t?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','4',N'Ha nem felelt meg, annak mi volt az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'H','5',N'Mennyire el�gedett �sszess�g�ben a Kontakt tolm�csszolg�ltat�ssal?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','1',N'Mennyire �rtetted a hall� �gyfelet tolm�csol�s k�zben (tapasztalhat�-e szakadoz�s, tiszt�n �rthet�-e a hang�stb.)?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','2',N'Ha nehezen �rtetted a hall� �gyfelet, annak mi az oka?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','3',N'Mennyire volt k�vethet� sz�modra a hall�ss�r�lt �gyf�l?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','4',N'Mennyire l�that� j�l a hall�ss�r�lt �gyf�l a tolm�csol�s ideje alatt (mimika l�that�s�ga, k�zmozg�sok l�that�s�ga, szakadozik-e a k�p� stb.)?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','5',N'Melyik akad�lymentes�t�si m�dot r�szes�ted el�nyben?')"));
            Sql(string.Format("INSERT INTO SurveyQuestionCodes VALUES (newid(),'J','6',N'Mennyire vagy el�gedett �sszess�g�ben a Kontakt tolm�csszolg�ltat�ssal?')"));

            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','1','a',N'azonnal siker�lt csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','1','b',N't�bbsz�ri pr�b�lkoz�s ut�n siker�lt csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','a',N'kiv�l� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','b',N'j� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','c',N'k�zepes min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','d',N'megfelel� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','2','e',N'nem felelt meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','a',N'k�l�nb�z� jelhaszn�lat')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','b',N'ford�t�s gyorsas�ga')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','c',N'ismeretlen sz�momra a tolm�cs')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','d',N'sz�momra nem �rthet�, �j jelek')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','3','e',N'technikai okok (nem l�tom rendesen a tolm�csot, szakadozik a k�p, kicsi a tolm�csablak, rossz a megvil�g�t�s)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','a',N'kiv�l�an l�that�, k�vethet�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','b',N'j�l l�that�, k�vethet�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','c',N'r�szben j�l l�that�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','d',N'rosszul l�that�, de m�g k�vethet�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','4','e',N'nagyon rosszul l�that�, k�vethetetlen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','a',N'nagyon el�gedett vagyok, sz�vesen haszn�ln�m rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','b',N'el�gedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','c',N'elfogadhat�, de m�g szokatlan')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','5','d',N'nem vagyok el�gedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','a',N'korszer� akad�lymentes�t�si m�d')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','b',N'k�nyelmes megold�s sz�momra')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','c',N'id�t takar�tok meg vele')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','d',N'j�, hogy nem kell utaznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','e',N'el�ny�s, hogy bizalmas helyzetekben nincs jelen a tolm�cs (pl. orvosn�l)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'U','6','f',N'el�ny�s, hogy b�rhonnan el�rhet� a szolg�ltat�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','1','a',N'azonnal siker�lt csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','1','b',N't�bbsz�ri pr�b�lkoz�s ut�n siker�lt csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','2','a',N'jelnyelvi tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','2','b',N'�r�tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','a',N'kiv�l� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','b',N'j� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','c',N'k�zepes min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','d',N'megfelel� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','3','e',N'nem felelt meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','a',N'a jelnyelvi/�r�tolm�csol�s temp�ja')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','b',N'a tolm�csol�s akadoz�sa')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','c',N'a tolm�csol�s pontatlans�ga')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','4','d',N'technikai akad�lyok (szakadoz�s tapasztalhat�, nem �rthet� tiszt�n a hang, nem olvashat� megfelel�en a sz�veg�stb.)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','a',N'nagyon el�gedett vagyok, sz�vesen haszn�ln�m rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','b',N'el�gedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','c',N'elfogadhat�, de m�g szokatlan')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'H','5','d',N'nem vagyok el�gedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','a',N'K�zhatalmat gyakorl� szerv elj�r�sa sor�n v�gzett tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','b',N'Eg�szs�g�gyi ell�t�s ig�nybev�telekor v�gzett tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','c',N'Foglalkoztat�si c�l� �s munkahelyen ig�nybe vett tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','d',N'A jogosult v�lasztott tiszts�g�nek ell�t�s�hoz ig�nybe vett tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','e',N'A jogosult �n�ll� �letvitel�nek el�seg�t�se c�lj�b�l v�gzett tolm�csol�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'HE','1','f',N'S�rg�ss�gi t�vtolm�csol�s (seg�lyh�v�s)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','1','a',N'azonnal siker�lt csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','1','b',N't�bbsz�ri pr�b�lkoz�s ut�n siker�lt csatlakoznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','a',N'kiv�l� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','b',N'j� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','c',N'k�zepes min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','d',N'megfelel� min�s�g� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','2','e',N'nem felelt meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','a',N'a felhaszn�l�i fel�let nem k�nyelmes (k�perny�, bet�m�ret, bet�st�lus, sz�nek�stb.)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','b',N'a megjelen� sz�veg nem folyamatos')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','c',N'a tolm�csol�s temp�ja')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','3','d',N'sok az el�t�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','a',N'nagyon el�gedett vagyok, sz�vesen haszn�ln�m rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','b',N'el�gedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','c',N'elfogadhat�, de m�g szokatlan')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','4','d',N'nem vagyok el�gedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','a',N'korszer� akad�lymentes�t�si m�d')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','b',N'k�nyelmes megold�s sz�momra')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','c',N'id�t takar�tok meg vele')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','d',N'j�, hogy nem kell utaznom')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','e',N'el�ny�s, hogy bizalmas helyzetekben nincs jelen a tolm�cs (pl. orvosn�l)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'I','5','f ',N'el�ny�s, hogy b�rhonnan el�rhet� a szolg�ltat�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','a',N'kiv�l�an �rtettem mindent')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','b',N'j�l �rtettem')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','c',N'nem �rtettem mindent')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','d',N'keveset �rtettem meg')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','1','e',N'semmit nem �rtettem')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','a',N'a besz�dtemp� gyorsas�ga')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','b',N'besz�dtemp� lass�s�ga')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','c',N'm�szaki ok (szakadozik a vonal, nem �rthet� a hall� �gyf�l besz�de�stb.)')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','2','d',N't�l sok idegen kifejez�s')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','a',N'kiv�l�an �rtettem mindent, nagyon j�l k�vethet� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','b',N'j�l �rtettem, k�vethet� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','c',N'nem �rtettem mindent, r�szben j�l k�vethet� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','d',N'keveset �rtettem meg, de m�g k�vethet� volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','3','e',N'semmit nem �rtettem, k�vethetetlen volt')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','a',N'kiv�l�an, pontosan l�that�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','b',N'j�l l�that�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','c',N'nem l�that� j�l')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','d',N'rosszul l�that�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','4','e',N'nagyon rosszul l�that�')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','a',N'Az �r�tolm�csol�st')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','b',N'A jelnyelvi tolm�csol�st')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','c',N'mindkett�t')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','5','d',N'mindegy')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','a',N'nagyon el�gedett vagyok, sz�vesen haszn�ln�m rendszeresen')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','b',N'el�gedett vagyok')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','c',N'k�zepesen vagyok el�gedett')"));
            Sql(string.Format("INSERT INTO SurveyAnswerCodes VALUES (newid(),'J','6','d',N'nem vagyok el�gedett')"));

        }
        
        public override void Down()
        {
            DropTable("dbo.SurveyQuestionCodes");
            DropTable("dbo.SurveyAnswerCodes");
        }
    }
}
