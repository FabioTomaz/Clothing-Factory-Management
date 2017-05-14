USE [GESTAO-FABRICA-VESTUARIO-LABORAL]
GO

--DROP TABLE [FABRICA-FILIAL]

 --reset IDENTITY: 
 --DBCC CHECKIDENT ('MATERIAIS_TÊXTEIS', RESEED, 0);

INSERT INTO ZONA (COD_POSTAL, DISTRITO, CONCELHO, LOCALIDADE) VALUES
	('4558-547', 'Porto', 'Gaia', 'Gaia'),
	('1254-585', 'Viseu', 'Ali', 'Acolá'),
	('1248-452', 'Lisboa', 'Sintra', 'Sintra'),
	('2288-227', 'Aveiro', 'Àgueda', 'Beco'),
	('3865-229', 'Aveiro', 'Estarreja', 'Salreu');

INSERT INTO CLIENTE (NOME, NIB, NIF, EMAIL, TELEMOVEL, COD_POSTAL, RUA, N_PORTA) VALUES
	('Bruno Silva', 784526412578965412354, 222444489, 'bjpsilva@hotmail.com', 919225360, '3865-229', 'Maria de Lurdes Breu', 10),
	('Bruno Pires', 784526412548965412354, 222544489, 'bjpsilva533@gmail.com', 919225361, '3865-229', 'Avenida da Liberdade', 25),
	('Fabio Santos', 452178521452014789652, 888999555, 'fmts@ua.pt', 967711447, '2288-227', 'Rua do Beco', 15);
	
INSERT INTO [FABRICA-FILIAL] (EMAIL, TELEFONE, FAX, COD_POSTAL, RUA, N_PORTA) VALUES
	('ftextil@mail.com', 92546545, 23585588, '1248-452', 'Rua das favelas', 15);

INSERT INTO [TIPO-UTILIZADOR] (TIPO) VALUES
	('Gestor de Stock'),	--1
	('Gestor de Produção'),	--2
	('Gestor de Vendas'),		--3
	('Gestor de Recursos Humanos');	--4
	
INSERT INTO UTILIZADOR (NOME, EMAIL, SALARIO, PASS, TELEFONE, N_FABRICA, 
HORA_ENTRADA, HORA_SAIDA, COD_POSTAL, RUA, N_PORTA, N_FUNCIONARIO_SUPER) VALUES
	('Carlos Costa', 'costa@mail.pt', 2000, 'costatoy', 91547855, 1, '09:00:00 AM', '18:00:00 PM', '2288-227', 'rua da Beira', 17, 1),
	('António Cruz', 'acruz@mail.pt', 1000, 'cruz33', 925447557, 1, '09:30:00 AM', '18:00:00 PM', '3865-229', 'rua da Feira', 12, 1),
	('Rui Jorge', 'rjorge@mail.pt', 1200, 'jorge33', 91475557, 1, '09:30:00 AM', '18:00:00 PM', '3865-229', 'rua da caridade', 12, 1),
	('José Pacheco', 'jospach@mail.pt', 1500, 'pch45', 91511710, 1, '09:00:00 AM', '17:00:00 PM', '3865-229', 'rua Maria Breu', 20, 1);

INSERT INTO [UTILIZADOR-TIPOS](UTILIZADOR, ID_TIPO) VALUES
	(1,4),
	(2,2),
	(3,3),
	(4,1);


INSERT INTO ENCOMENDA (DATA_CONFIRMACAO, DATA_ENTREGA_PREV, LOCALENTREGA, ESTADO, CLIENTE, N_GESTOR_VENDA) VALUES
	('20170520', '20170602', NULL, 1, 1, 3),
	('20170515', '20170602', NULL, 2, 2, 3);
	
	
INSERT INTO [PRODUTO-BASE] (NOME, DATA_ALTERACAO, IVA ,INSTRUCOES_PRODUCAO, N_GESTOR_PROD, IMAGEM_DESENHO) VALUES
	('Bata azul', '20170424', 23.0 ,'Usar linha e tecido e pintar de azul', 2, 'image'),
	('Colete', '20170511', 23.0 ,'Usar pano e linha e elásticos', 2, 'image');

INSERT INTO ETIQUETA (NORMAS, PAIS_FABRICO, COMPOSICAO) VALUES
	('Mais exemplos de normas', 'Espanha', 'Pano, linha'),
	('normas e tal', 'Portugal', 'algodão, linho');
	
INSERT INTO [PRODUTO-PERSONALIZADO] (REFERENCIA, TAMANHO, COR, PRECO, UNIDADES_ARMAZEM, N_ETIQUETA) VALUES
	(1, 'XL', 'azul-escuro', 17.99, 1, 1);
	
INSERT INTO CONTEUDO_ENCOMENDA (N_ENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO, ID_PRODUTO ,QUANTIDADE) VALUES
	(1, 1, 'XL', 'azul-escuro', 1, 2);
	

INSERT INTO FORNECEDOR (NIF, EMAIL, NOME, FAX, TELEFONE, DESIGNACAO, COD_POSTAL, RUA, N_PORTA) VALUES
	(666999555, 'for_textil@mail.pt','Textil suply', 223415478, 22485497, 'Fornecimento de panos e linho para produçao têxtil', '1254-585', 'Rua das produções', 23),
	(478545217, 'forn_materiais@mail.pt', 'Textil material inc.', 222454485, 2135748, 'Fornecimento de materiais para produçao textil', '4558-547', 'Rua do trabalho', 20);

INSERT INTO MATERIAIS_TÊXTEIS (REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO) VALUES
	(300, 478545217, 'Azul', 'Pano linho'),
	(302, 666999555, 'Preto', 'Botão casaco'),
	(303, 666999555, 'Castanho', 'Mola'),
	(301, 478545217, 'Branco', 'Linha'),
	(304, 666999555, 'Amarelo', 'Elastico');