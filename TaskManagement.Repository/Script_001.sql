﻿IF(SELECT DB_ID('EluminiTest')) IS NULL
BEGIN
CREATE DATABASE EluminiTest;
END

USE EluminiTest;


IF (SELECT OBJECT_ID('Tb_Task')) IS NULL
BEGIN
	CREATE TABLE Tb_Task (
		Id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID(),
		[Description] VARCHAR(100) NOT NULL,
		[Status] VARCHAR(20) NOT NULL,
		[Date] DATETIME NOT NULL,
		[CreatedAt] DATETIME NOT NULL,
		[UpdatedAt] DATETIME NOT NULL,
		CONSTRAINT PK_TipoCliente_ID PRIMARY KEY (Id)
	);
END


select * from Tb_Task