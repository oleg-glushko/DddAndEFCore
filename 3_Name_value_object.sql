USE [EFCoreDDD]
GO
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.Student.Name', N'Tmp_FirstName', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.Student.Tmp_FirstName', N'FirstName', 'COLUMN' 
GO
ALTER TABLE dbo.Student ADD
	LastName nvarchar(50) NOT NULL CONSTRAINT DF_Student_LastName DEFAULT ''
GO
ALTER TABLE dbo.Student
	DROP CONSTRAINT DF_Student_LastName
GO
ALTER TABLE dbo.Student SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

UPDATE Student SET LastName = 'Alison' WHERE FirstName = 'Alice'
GO
UPDATE Student SET LastName = 'Bobson' WHERE FirstName = 'Bob'
GO
UPDATE Student SET LastName = 'Carlson' WHERE FirstName = 'Carl'
GO
