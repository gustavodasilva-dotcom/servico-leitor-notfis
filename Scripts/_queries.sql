DROP DATABASE IF EXISTS DB_Leitor_Arquivos;
CREATE DATABASE DB_Leitor_Arquivos;
GO

USE DB_Leitor_Arquivos;
GO


-- Criada
DROP TABLE IF EXISTS Extensoes
CREATE TABLE Extensoes
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Nome			VARCHAR(100)
	,Extensao		VARCHAR(20)
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_ExtensaoID PRIMARY KEY(ID)
);

INSERT INTO Extensoes (Nome, Extensao) VALUES ('Arquivo TXT', '*.txt');
INSERT INTO Extensoes (Nome, Extensao) VALUES ('Arquivo CSV', '*.csv');
INSERT INTO Extensoes (Nome, Extensao) VALUES ('Arquivo XML', '*.xml');


-- Criada
DROP TABLE IF EXISTS Layouts
CREATE TABLE Layouts
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Descricao		VARCHAR(20)
	,ExtensaoID		INT
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_LayoutID PRIMARY KEY(ID)

	CONSTRAINT FK_Extensao_LayoutID FOREIGN KEY(ExtensaoID)
	REFERENCES Extensoes(ID)
);

INSERT INTO Layouts (Descricao, ExtensaoID) VALUES ('Proceda 3.1', 1);


--Criada
DROP TABLE IF EXISTS Clientes
CREATE TABLE Clientes
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Nome			VARCHAR(200)
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_ClienteID PRIMARY KEY(ID)
);

INSERT INTO Clientes (Nome) VALUES ('Cliente fictício NOTFIS');


--Criada
DROP TABLE IF EXISTS Clientes_Layouts
CREATE TABLE Clientes_Layouts
(
	 ID				INT			NOT NULL IDENTITY(1, 1)
	,ClienteID		INT
	,LayoutID		INT
	,Ativo			BIT			DEFAULT 1
	,Excluido		BIT			DEFAULT 0
	,Data			DATETIME	DEFAULT GETDATE()

	CONSTRAINT PK_Cliente_LayoutID PRIMARY KEY(ID)

	CONSTRAINT FK_Clientes_Layouts_ClienteID FOREIGN KEY(ClienteID)
	REFERENCES Clientes(ID),

	CONSTRAINT FK_Clientes_Layouts_LayoutID FOREIGN KEY(LayoutID)
	REFERENCES Layouts(ID)
);

INSERT INTO Clientes_Layouts (ClienteID, LayoutID) VALUES (1, 1);


DROP TABLE IF EXISTS PastasClientes
CREATE TABLE PastasClientes
(
	 ID						INT				NOT NULL IDENTITY(1, 1)
	,Cliente_LayoutID		INT
	,CaminhoNovosArquivos	VARCHAR(300)
	,CaminhoProcessados		VARCHAR(400)
	,CaminhoErro			VARCHAR(400)
	,Ativo					BIT				DEFAULT 1
	,Excluido				BIT				DEFAULT 0
	,Data					DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_PastaClienteID PRIMARY KEY(ID)

	CONSTRAINT FK_PastasClientes_Cliente_LayoutID FOREIGN KEY(Cliente_LayoutID)
	REFERENCES Clientes_Layouts(ID)
);

INSERT INTO PastasClientes
(
	 Cliente_LayoutID
	,CaminhoNovosArquivos
	,CaminhoProcessados
	,CaminhoErro
)
VALUES
(
	 1
	,'C:\Leitor\ClienteFicticioNOTFIS\NEW\'
	,'C:\Leitor\ClienteFicticioNOTFIS\PROCESSED\'
	,'C:\Leitor\ClienteFicticioNOTFIS\ERROR\'
);