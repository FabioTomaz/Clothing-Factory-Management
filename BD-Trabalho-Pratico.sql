
CREATE DATABASE [GESTAO-FABRICA-VESTUARIO-LABORAL]
GO

USE [GESTAO-FABRICA-VESTUARIO-LABORAL]
GO

CREATE TABLE ZONA(
	COD_POSTAL			CHAR(8)			PRIMARY KEY,
	DISTRITO			VARCHAR(20)		NOT NULL,
	CONCELHO			VARCHAR(20)		NOT NULL,
	LOCALIDADE			VARCHAR(30)		NOT NULL
);

CREATE TABLE CLIENTE(
	NCLIENTE			INT				PRIMARY KEY IDENTITY(1,1),
	NIF					CHAR(9)			UNIQUE,
	NOME				VARCHAR(50)		NOT NULL,
	NIB					CHAR(21)		UNIQUE,
	EMAIL				VARCHAR(30)		UNIQUE,
	TELEMOVEL			VARCHAR(22)		UNIQUE NOT NULL,
	COD_POSTAL			CHAR(8)			NOT NULL REFERENCES ZONA(COD_POSTAL),
	RUA					VARCHAR(40)		NOT NULL,
	N_PORTA				INT				NOT NULL CHECK(N_PORTA>0)
);
CREATE TABLE [FABRICA-FILIAL](
	N_FILIAL			INT				PRIMARY KEY	IDENTITY(1,1),
	EMAIL				VARCHAR(50)		UNIQUE	NOT NULL,
	TELEFONE			VARCHAR(22)		UNIQUE	NOT NULL,
	FAX					VARCHAR(22)		UNIQUE,
	COD_POSTAL			CHAR(8)			NOT NULL REFERENCES ZONA(COD_POSTAL),
	RUA					VARCHAR(40)		NOT NULL,
	N_PORTA				INT				NOT NULL CHECK(N_PORTA>0)
);

/*1 - Gestor de Stock
2 - Gestor de Produ��o
3 - Gestor de Vendas
4 - Gestor de Recursos Humanos */
CREATE TABLE [TIPO-UTILIZADOR](
	ID					INT				PRIMARY KEY IDENTITY(1,1),
	TIPO				VARCHAR(50)		UNIQUE NOT NULL
);

CREATE TABLE UTILIZADOR(
	N_FUNCIONARIO		INT				PRIMARY KEY	IDENTITY(1,1),
	EMAIL				VARCHAR(40)		UNIQUE	NOT NULL,
	SALARIO				DECIMAL(6,2)	NOT NULL CHECK(SALARIO>=0),
	NOME				VARCHAR(50)		NOT NULL,
	PASS				VARCHAR(30)		NOT NULL,
	TELEFONE			VARCHAR(22)		UNIQUE	NOT NULL,
	N_FABRICA			INT				NOT NULL REFERENCES [FABRICA-FILIAL](N_FILIAL),
	HORA_ENTRADA		TIME			NOT NULL,
	HORA_SAIDA			TIME			NOT NULL,
	COD_POSTAL			CHAR(8)			NOT NULL REFERENCES ZONA(COD_POSTAL),
	RUA					VARCHAR(40)		NOT NULL,
	N_PORTA				INT				NOT NULL CHECK(N_PORTA>0),
	IMAGEM				IMAGE			,
	CHECK(HORA_ENTRADA < HORA_SAIDA)
);

CREATE TABLE [UTILIZADOR-TIPOS](
	UTILIZADOR			INT				REFERENCES UTILIZADOR(N_FUNCIONARIO),
	ID_TIPO				INT				REFERENCES [TIPO-UTILIZADOR](ID),
	PRIMARY KEY(UTILIZADOR, ID_TIPO)
);

CREATE TABLE ESTADO(
	ID					INT				PRIMARY KEY IDENTITY(1,1),
	DESCRI�AO			VARCHAR(50)		UNIQUE NOT NULL
);


CREATE TABLE ENCOMENDA(
	N_ENCOMENDA			INT 			IDENTITY(1,1) PRIMARY KEY,
	DATA_CONFIRMACAO	DATE			NOT NULL,
	DATA_ENTREGA		DATE			,
	DATA_ENTREGA_PREV	DATE			NOT NULL,
	LOCALENTREGA		VARCHAR(30),
	DESCONTO			DECIMAL(5,2)	NOT NULL DEFAULT 0.00 CHECK(DESCONTO>=0 AND DESCONTO<=100),
	ESTADO				INT				REFERENCES ESTADO(ID)	NOT NULL,
	CLIENTE				INT				NOT NULL REFERENCES CLIENTE(NCLIENTE) ON DELETE CASCADE ON UPDATE CASCADE,
	N_GESTOR_VENDA		INT				NOT NULL REFERENCES  [UTILIZADOR](N_FUNCIONARIO) ON UPDATE CASCADE
);


CREATE TABLE ETIQUETA(
	N_ETIQUETA			INT				PRIMARY KEY IDENTITY(1,1),
	NORMAS				VARCHAR(100)	NOT NULL,
	PAIS_FABRICO		VARCHAR(20)		NOT NULL,
	COMPOSICAO			VARCHAR(100)	NOT NULL
);

CREATE TABLE [PRODUTO-BASE](
	REFERENCIA			INT				IDENTITY(1,1),
	NOME				VARCHAR(50)		NOT NULL,
	IVA					DECIMAL(5,2)	NOT NULL DEFAULT 23.00,
	DATA_ALTERACAO		DATE			NOT NULL,
	INSTRUCOES_PRODUCAO	VARCHAR(200)	NOT NULL,
	N_GESTOR_PROD		INT				NOT NULL REFERENCES [UTILIZADOR](N_FUNCIONARIO) ON UPDATE CASCADE,
	IMAGEM_DESENHO		IMAGE			NOT NULL,
	PRIMARY KEY(REFERENCIA)
);

CREATE TABLE [PRODUTO-PERSONALIZADO](
	REFERENCIA			INT				REFERENCES [PRODUTO-BASE](REFERENCIA),
	TAMANHO				VARCHAR(5)		,
	COR					VARCHAR(15)		,
	ID					INT				IDENTITY(1,1),
	N_ETIQUETA			INT				REFERENCES ETIQUETA(N_ETIQUETA),
	PRECO				DECIMAL(7,2)	NOT NULL CHECK(PRECO>0),
	UNIDADES_ARMAZEM	INT				NOT NULL CHECK(UNIDADES_ARMAZEM>=0) DEFAULT 0,
	PRIMARY KEY(REFERENCIA, TAMANHO, COR, ID)
);

CREATE TABLE CONTEUDO_ENCOMENDA(
	N_ENCOMENDA			INT				REFERENCES ENCOMENDA(N_ENCOMENDA),
	REFERENCIA_PRODUTO	INT				,
	TAMANHO_PRODUTO		VARCHAR(5)		,
	COR_PRODUTO			VARCHAR(15)		,
	ID_PRODUTO			INT				,
	QUANTIDADE			INT				NOT NULL CHECK(QUANTIDADE>0) DEFAULT 1,
	FOREIGN KEY(REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO, ID_PRODUTO) REFERENCES [PRODUTO-PERSONALIZADO](REFERENCIA, TAMANHO, COR, ID),
	PRIMARY KEY(N_ENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO, ID_PRODUTO)
);

CREATE TABLE FORNECEDOR(
	NIF					CHAR(9)			PRIMARY KEY CHECK(NIF>0),
	EMAIL				VARCHAR(50)		NOT NULL UNIQUE,
	NOME				VARCHAR(30)		NOT NULL UNIQUE,
	FAX					VARCHAR(22)		,
	TELEFONE			VARCHAR(22)		NOT NULL,
	DESIGNACAO			VARCHAR(100)	NOT NULL,
	COD_POSTAL			CHAR(8)			NOT NULL REFERENCES ZONA(COD_POSTAL),
	RUA					VARCHAR(40)		NOT NULL,
	N_PORTA				INT				NOT NULL CHECK(N_PORTA>0),
);

CREATE TABLE BORDADO(
	REFERENCIA			INT				,
	TAMANHO				VARCHAR(5)		,
	COR					VARCHAR(15)		,
	ID					INT				,
	DESIGNACAO			VARCHAR(100)	NOT NULL,
	IMAGEM				IMAGE			,
	PRECO				DECIMAL(10,2)	NOT NULL CHECK(PRECO>0),
	LOCALIZACAO			VARCHAR(20)		NOT NULL,
	NIF_FORNECEDOR		CHAR(9)			NOT NULL REFERENCES FORNECEDOR(NIF) ON UPDATE CASCADE,
	QUALIDADE			INT				NOT NULL CHECK(QUALIDADE>0 AND QUALIDADE<=3),
	FOREIGN KEY ( REFERENCIA, TAMANHO, COR, ID ) REFERENCES	[PRODUTO-PERSONALIZADO](REFERENCIA, TAMANHO, COR, ID),
	PRIMARY KEY(REFERENCIA, TAMANHO, COR, ID)
);

CREATE TABLE ESTAMPAGEM(
	REFERENCIA			INT				,
	TAMANHO				VARCHAR(5)		,
	COR					VARCHAR(15)		,
	ID					INT				,
	DESIGNACAO			VARCHAR(100)	NOT NULL,
	IMAGEM				IMAGE			,
	PRECO				DECIMAL(10,2)	NOT NULL CHECK(PRECO>0),
	LOCALIZACAO			VARCHAR(20)		NOT NULL,
	NIF_FORNECEDOR		CHAR(9)			NOT NULL REFERENCES FORNECEDOR(NIF) ON UPDATE CASCADE,
	METODO				VARCHAR(30)		NOT NULL ,
	FOREIGN KEY ( REFERENCIA, TAMANHO, COR, ID ) REFERENCES	[PRODUTO-PERSONALIZADO](REFERENCIA, TAMANHO, COR, ID),
	PRIMARY KEY(REFERENCIA, TAMANHO, COR, ID)
);


CREATE TABLE MATERIAIS_T�XTEIS(
	REFERENCIA_FABRICA	INT				PRIMARY KEY	IDENTITY(1,1),
	REFERENCIA_FORN		INT				NOT NULL	CHECK (REFERENCIA_FORN>0),
	NIF_FORNECEDOR		CHAR(9)			NOT NULL	REFERENCES FORNECEDOR(NIF) ON DELETE CASCADE,
	COR					VARCHAR(20)		NOT NULL,
	DESIGNACAO			VARCHAR(80)		NOT NULL,
);

CREATE TABLE PANO(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES MATERIAIS_T�XTEIS(REFERENCIA_FABRICA) ON DELETE CASCADE,
	TIPO				VARCHAR(20)		NOT NULL,
	GRAMAGEM			INT				NOT NULL,
	AREA_ARMAZEM		DECIMAL(10,2)	NOT NULL CHECK (AREA_ARMAZEM>=0)	DEFAULT 0,
	PRECO_POR_M2		DECIMAL(10,2)	NOT NULL
);

CREATE TABLE LINHA(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES MATERIAIS_T�XTEIS(REFERENCIA_FABRICA) ON DELETE CASCADE,
	GROSSURA			INT				NOT NULL,
	COMPRIMENTO_ARMAZEM	DECIMAL(10,2)	NOT NULL DEFAULT 0.00,
	PRECO_CEM_METROS	DECIMAL(10,2)	NOT NULL
);

CREATE TABLE ACESSORIO(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES MATERIAIS_T�XTEIS(REFERENCIA_FABRICA) ON DELETE CASCADE,
	QUANTIDADE_ARMAZEM	INT				NOT NULL DEFAULT 0,
	PRECO_UNIDADE		DECIMAL(10,2)	NOT NULL
);

CREATE TABLE FECHO(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES ACESSORIO(REFERENCIA_FABRICA) ON DELETE CASCADE,
	COMPRIMENTO			DECIMAL(10,2)	NOT NULL,
	TAMANHO_DENTE		DECIMAL(10,2)	NOT NULL
);

CREATE TABLE MOLA(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES ACESSORIO(REFERENCIA_FABRICA) ON DELETE CASCADE,
	DIAMETRO			DECIMAL(10,2)	NOT NULL
);

CREATE TABLE BOTAO(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES ACESSORIO(REFERENCIA_FABRICA) ON DELETE CASCADE,
	DIAMETRO			DECIMAL(10,2)	NOT NULL
);
CREATE TABLE ELASTICO(
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES ACESSORIO(REFERENCIA_FABRICA) ON DELETE CASCADE,
	LARGURA				DECIMAL(10,2)	NOT NULL,
	COMPRIMENTO			DECIMAL(10,2)	NOT NULL
);
CREATE TABLE [FITA-VELCRO](
	REFERENCIA_FABRICA	INT				PRIMARY KEY REFERENCES ACESSORIO(REFERENCIA_FABRICA) ON DELETE CASCADE,
	LARGURA				DECIMAL(10,2)	NOT NULL,
	COMPRIMENTO			DECIMAL(10,2)	NOT NULL
);

CREATE TABLE [MATERIAIS-PRODUTO](
	REFERENCIA			INT				,
	TAMANHO				VARCHAR(5)		,
	COR					VARCHAR(15)		,
	ID					INT				,
	REFERENCIA_FABRICA	INT				REFERENCES MATERIAIS_T�XTEIS(REFERENCIA_FABRICA) ON DELETE CASCADE,
	QUANTIDADE			INT				NOT NULL CHECK(QUANTIDADE>=0),
	FOREIGN KEY ( REFERENCIA, TAMANHO, COR, ID )	REFERENCES [PRODUTO-PERSONALIZADO]( REFERENCIA, TAMANHO, COR, ID ),
	PRIMARY KEY(REFERENCIA, TAMANHO, COR, ID, REFERENCIA_FABRICA)
);

ALTER TABLE UTILIZADOR ADD N_FUNCIONARIO_SUPER	INT	REFERENCES [UTILIZADOR](N_FUNCIONARIO);


/* 1 - Aceite
2 - Pendente de materiais para produ��o
3 - Em processo de fabrico
4 - Pronto para entrega
5 - Entregue
*/
INSERT INTO ESTADO(DESCRI�AO) VALUES('Aceite');
INSERT INTO ESTADO(DESCRI�AO) VALUES('Pendente de materiais para produ��o');
INSERT INTO ESTADO(DESCRI�AO) VALUES('Em processo de fabrico');
INSERT INTO ESTADO(DESCRI�AO) VALUES('Pronto para entrega');
INSERT INTO ESTADO(DESCRI�AO) VALUES('Entregue');
