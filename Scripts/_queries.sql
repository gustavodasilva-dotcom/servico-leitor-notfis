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


-- Criada
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


--Criada
DROP TABLE IF EXISTS Tags
CREATE TABLE Tags
(
	 ID				INT				NOT NULL IDENTITY(1,1)
	,Nome			VARCHAR(100)	NOT NULL
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_TagID PRIMARY KEY(ID)
);

INSERT INTO Tags
(
Nome
)
VALUES
 ('RemetenteIdentificacao')
,('RemetenteInscricaoEstadual')
,('RemetenteRazaoSocial')
,('DestinatarioNome')
,('DestinatarioIdentificacao')
,('DestinatarioInscricaoEstadual')
,('RemetenteCep')
,('RemetenteLogradouro')
,('RemetenteNumero')
,('RemetenteComplemento')
,('RemetenteBairro')
,('RemetenteCidade')
,('RemetenteEstado')
,('DestinatarioCep')
,('DestinatarioLogradouro')
,('DestinatarioNumero')
,('DestinatarioComplemento')
,('DestinatarioBairro')
,('DestinatarioCidade')
,('DestinatarioEstado')
,('NumeroOrdem')
,('Preco')
,('ChaveNFe')
,('Quantidade')
,('Descricao');


--Criada
DROP TABLE IF EXISTS Clientes_Layouts_Tag
CREATE TABLE Clientes_Layouts_Tag
(
	 ID					INT				NOT NULL IDENTITY(1,1)
	,TagID				INT				NOT NULL
	,Obrigatoria		BIT				NOT NULL
	,InicioIndice		INT
	,Tamanho			INT
	,Attribute			VARCHAR(150)
	,Indice				INT
	,Cliente_LayoutID	INT				NOT NULL
	,Ativo				BIT				DEFAULT 1
	,Excluido			BIT				DEFAULT 0
	,Data				DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_Cliente_Layout_TagID PRIMARY KEY(ID)

	CONSTRAINT FK_Clientes_Layouts_Tag_Cliente_LayoutID FOREIGN KEY(Cliente_LayoutID)
	REFERENCES Clientes_Layouts(ID),

	CONSTRAINT FK_Clientes_Layouts_Tag_TagsID FOREIGN KEY(TagID)
	REFERENCES Tags(ID)
);

INSERT INTO Clientes_Layouts_Tag
(
	 TagID
	,Obrigatoria
	,InicioIndice
	,Tamanho
	,Attribute
	,Indice
	,Cliente_LayoutID
)
VALUES
 (1, 1, 3, 14, null, null, 1)
,(2, 0, null, null, null, null, 1)
,(3, 1, 133, 35, null, null, 1)
,(4, 1, 3, 34, null, null, 1)
,(5, 1, 43, 11, null, null, 1)
,(6, 1, 57, 6, null, null, 1)
,(7, 1, 107, 8, null, null, 1)
,(8, 1, 32, 24, null, null, 1)
,(9, 1, 173, 3, null, null, 1)
,(10, 0, null, null, null, null, 1)
,(11, 0, null, null, null, null, 1)
,(12, 1, 72, 9, null, null, 1)
,(13, 1, 116, 2, null, null, 1)
,(14, 1, 167, 8, null, null, 1)
,(15, 1, 72, 19, null, null, 1)
,(16, 1, 294, 4, null, null, 1)
,(17, 1, 240, 12, null, null, 1)
,(18, 1, 112, 11, null, null, 1)
,(19, 1, 132, 8, null, null, 1)
,(20, 1, 185, 2, null, null, 1)
,(21, 1, 108, 11, null, null, 1)
,(22, 1, 78, 34, null, null, 1)
,(23, 1, 258, 44, null, null, 1)
,(24, 1, 3, 7, null, null, 1)
,(25, 1, 25, 37, null, null, 1);
