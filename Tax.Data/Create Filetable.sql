--olvasnivaló: #2062: http://egroup.azurewebsites.net/Inquiry/View/8fedc549-c206-436f-92d4-5a3a298ab030

--enable filestream
--ez nálam látszólag nem működött, ezért kézzel az SQL Configuration Manager-rel kellett
--engedélyezni a FileStream-et (PG)

/*
 1. SQL Server configuration - filestream bekapcsolása (transact sql + io access)
 2. SQL Service restart
 3. Futtatás
*/

--create directory
ALTER DATABASE kontaktdb
SET FILESTREAM (NON_TRANSACTED_ACCESS = FULL, DIRECTORY_NAME = 'FilesShare')
GO


----ALTER DATABASE kontaktdb REMOVE FILE kontaktfiles
----ALTER DATABASE kontaktdb REMOVE FILEGROUP kontaktfg
--create filegroup
ALTER DATABASE kontaktdb
ADD FILEGROUP kontaktfg CONTAINS FILESTREAM
GO
--create file
ALTER DATABASE kontaktdb
ADD FILE (
	Name = kontaktfiles,
	FILENAME = 'E:\MSSQL11.MSSQLSERVER\MSSQL\Filestream'
)
TO FILEGROUP kontaktfg
GO

--create filetable
CREATE TABLE Files AS FILETABLE
	WITH ( 
			FileTable_Directory = 'FilesShare',
			FileTable_Collate_Filename = database_default
			);
GO
