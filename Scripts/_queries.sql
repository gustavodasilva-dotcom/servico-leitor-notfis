DROP DATABASE IF EXISTS DB_Leitor_Arquivos;
CREATE DATABASE DB_Leitor_Arquivos;
GO

USE DB_Leitor_Arquivos;
GO


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


-- Criada
DROP TABLE IF EXISTS PastasClientes
CREATE TABLE PastasClientes
(
	 ID						INT				NOT NULL IDENTITY(1, 1)
	,ClienteID				INT
	,CaminhoNovosArquivos	VARCHAR(300)
	,CaminhoProcessados		VARCHAR(400)
	,CaminhoErro			VARCHAR(400)
	,Ativo					BIT				DEFAULT 1
	,Excluido				BIT				DEFAULT 0
	,Data					DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_PastaClienteID PRIMARY KEY(ID)

	CONSTRAINT FK_PastasClientes_ClienteID FOREIGN KEY(ClienteID)
	REFERENCES Clientes(ID)
);

INSERT INTO PastasClientes
(
	 ClienteID
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
DROP TABLE IF EXISTS Clientes_Tags
CREATE TABLE Clientes_Tags
(
	 ID					INT				NOT NULL IDENTITY(1,1)
	,TagID				INT				NOT NULL
	,Obrigatoria		BIT				NOT NULL
	,IndiceInicial		INT
	,Tamanho			INT
	,ClienteID			INT				NOT NULL
	,Ativo				BIT				DEFAULT 1
	,Excluido			BIT				DEFAULT 0
	,Data				DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_Cliente_TagID PRIMARY KEY(ID)

	CONSTRAINT FK_Clientes_Tag_ClienteID FOREIGN KEY(ClienteID)
	REFERENCES Clientes(ID),

	CONSTRAINT FK_Clientes_Tag_TagID FOREIGN KEY(TagID)
	REFERENCES Tags(ID)
);

INSERT INTO Clientes_Tags
(
	 TagID
	,Obrigatoria
	,IndiceInicial
	,Tamanho
	,ClienteID
)
VALUES
 (1, 1, 3, 14, 1)
,(2, 0, null, null, 1)
,(3, 1, 133, 35, 1)
,(4, 1, 3, 34, 1)
,(5, 1, 43, 11, 1)
,(6, 1, 57, 6, 1)
,(7, 1, 107, 8, 1)
,(8, 1, 32, 24, 1)
,(9, 1, 173, 3, 1)
,(10, 0, null, null, 1)
,(11, 0, null, null, 1)
,(12, 1, 72, 9, 1)
,(13, 1, 116, 2, 1)
,(14, 1, 167, 8, 1)
,(15, 1, 72, 19, 1)
,(16, 1, 294, 4, 1)
,(17, 1, 240, 12, 1)
,(18, 1, 112, 11, 1)
,(19, 1, 132, 8, 1)
,(20, 1, 185, 2, 1)
,(21, 1, 108, 11, 1)
,(22, 1, 78, 34, 1)
,(23, 1, 258, 44, 1)
,(24, 1, 3, 7, 1)
,(25, 1, 25, 37, 1);