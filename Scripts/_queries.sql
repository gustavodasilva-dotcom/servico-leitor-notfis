DROP DATABASE IF EXISTS DB_Leitor_Arquivos;
CREATE DATABASE DB_Leitor_Arquivos;
GO

USE DB_Leitor_Arquivos;
GO


-- Criada
DROP TABLE IF EXISTS Layouts
CREATE TABLE Layouts
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Descricao		VARCHAR(50)
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_LayoutID PRIMARY KEY(ID)
);

INSERT INTO Layouts
(
	Descricao
)
VALUES
 ('Proceda 3.1')
,('Proceda 5.0');


--Criada
DROP TABLE IF EXISTS Linhas
CREATE TABLE Linhas
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Linha			VARCHAR(10)
	,LayoutID		INT
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_LinhaID PRIMARY KEY(ID)

	CONSTRAINT FK_Linhas_LayoutID FOREIGN KEY(LayoutID)
	REFERENCES Layouts(ID)
);

INSERT INTO Linhas
(
	 Linha
	,LayoutID
)
VALUES
 ('311', 1)
,('312', 1)
,('307', 1)
,('313', 1)
,('314', 1)
,('315', 1);


-- Criada
DROP TABLE IF EXISTS InformacoesLinhas
CREATE TABLE InformacoesLinhas
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Descricao		VARCHAR(50)
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_InformacaoLinhaID PRIMARY KEY(ID)
);

INSERT INTO InformacoesLinhas
(
	Descricao
)
VALUES
 ('Remetente')
,('Destinatario')
,('InformacoesOrdem1')
,('InformacoesOrdem2')
,('Items')
,('Quebra');


--Criada
DROP TABLE IF EXISTS Linhas_InformacoesLinhas
CREATE TABLE Linhas_InformacoesLinhas
(
	 ID					INT				NOT NULL IDENTITY(1, 1)
	,LinhaID			INT
	,InformacaoLinhaID	INT
	,Ativo				BIT				DEFAULT 1
	,Excluido			BIT				DEFAULT 0
	,Data				DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_Linha_InformacaoLinhaID PRIMARY KEY(ID)

	CONSTRAINT FK_Linhas_InformacaoLinha_LinhaID FOREIGN KEY(LinhaID)
	REFERENCES Linhas(ID),

	CONSTRAINT FK_Linhas_InformacaoLinha_InformacaoLinhaID FOREIGN KEY(InformacaoLinhaID)
	REFERENCES InformacoesLinhas(ID)
);

INSERT INTO Linhas_InformacoesLinhas (LinhaID, InformacaoLinhaID) VALUES (1, 1);
INSERT INTO Linhas_InformacoesLinhas (LinhaID, InformacaoLinhaID) VALUES (2, 2);
INSERT INTO Linhas_InformacoesLinhas (LinhaID, InformacaoLinhaID) VALUES (3, 3);
INSERT INTO Linhas_InformacoesLinhas (LinhaID, InformacaoLinhaID) VALUES (4, 4);
INSERT INTO Linhas_InformacoesLinhas (LinhaID, InformacaoLinhaID) VALUES (5, 5);
INSERT INTO Linhas_InformacoesLinhas (LinhaID, InformacaoLinhaID) VALUES (6, 6);


--Criada
DROP TABLE IF EXISTS Clientes
CREATE TABLE Clientes
(
	 ID				INT				NOT NULL IDENTITY(1, 1)
	,Nome			VARCHAR(200)
	,LayoutID		INT
	,Ativo			BIT				DEFAULT 1
	,Excluido		BIT				DEFAULT 0
	,Data			DATETIME		DEFAULT GETDATE()

	CONSTRAINT PK_ClienteID PRIMARY KEY(ID)

	CONSTRAINT FK_Clientes_LayoutID FOREIGN KEY(LayoutID)
	REFERENCES Layouts(ID)
);

INSERT INTO Clientes (Nome, LayoutID) VALUES ('Cliente fictício NOTFIS', 1);


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



-- SELECT para selecionar o layout de um cliente:
SELECT IL.Descricao,
       LN.*
FROM   Clientes C (nolock)
       INNER JOIN Layouts L (nolock)
               ON C.LayoutID = L.ID
       INNER JOIN Linhas LN (nolock)
               ON LN.LayoutID = L.ID
       INNER JOIN Linhas_InformacoesLinhas LI (nolock)
               ON LI.LinhaID = LN.ID
       INNER JOIN InformacoesLinhas IL (nolock)
               ON LI.InformacaoLinhaID = IL.ID
WHERE  C.ID = 1;