--enable filestream
--ez nálam látszólag nem működött, ezért kézzel az SQL Configuration Manager-rel kellett
--engedélyezni a FileStream-et (PG)

/*
 1. SQL Server configuration - filestream bekapcsolása (transact sql + io access)
 2. SQL Service restart
 3. Futtatás
*/
EXEC sp_configure filestream_access_level, 2
RECONFIGURE

--create directory
ALTER DATABASE taxdb
SET FILESTREAM (DIRECTORY_NAME = 'FilesShare')
GO

----ALTER DATABASE taxdb REMOVE FILE taxfiles
----ALTER DATABASE taxdb REMOVE FILEGROUP taxfg
--create filegroup
ALTER DATABASE taxdb
ADD FILEGROUP taxfg CONTAINS FILESTREAM
GO
--create file
ALTER DATABASE taxdb
ADD FILE (
	Name = taxfiles,
	FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Filestream'
)
TO FILEGROUP taxfg
GO

--create filetable
CREATE TABLE Files AS FILETABLE
	WITH ( 
			FileTable_Directory = 'FilesShare',
			FileTable_Collate_Filename = database_default
			);
GO
