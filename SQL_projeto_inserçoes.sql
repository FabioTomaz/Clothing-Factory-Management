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
	('Bata azul', '20170424', 23.0 ,'Começar a trabalhar pelas mangas juntar os punhos...', 2, 'image'),
	('Colete', '20170511', 23.0 ,'Comece por cozer as duas peças das costas adicione as mangas...', 2, 'image');
	
INSERT INTO ETIQUETA (NORMAS, PAIS_FABRICO, COMPOSICAO) VALUES
	('Lavagem a seco', 'Portugal', '60% algodão 30% polyester'),
	('Não Lavar com roupa branca', 'Portugal', '35% algodão 20% polyester');
	
INSERT INTO [PRODUTO-PERSONALIZADO] (REFERENCIA, TAMANHO, COR, PRECO, UNIDADES_ARMAZEM, N_ETIQUETA) VALUES
	(1, 'XL', 'azul escuro', 17.99, 1, 1),
	(1, 'XL', 'verde lima', 16.99,3, 1),
	(2, 'M', 'azul claro', 25.50, 0 ,2);
	
INSERT INTO CONTEUDO_ENCOMENDA (N_ENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO, ID_PRODUTO ,QUANTIDADE) VALUES
	(1, 1, 'XL', 'azul escuro', 1, 5),
	(1, 1, 'XL', 'verde lima', 2, 1),
	(2, 2, 'M', 'azul claro', 3, 2);
	

INSERT INTO FORNECEDOR (NIF, EMAIL, NOME, FAX, TELEFONE, DESIGNACAO, COD_POSTAL, RUA, N_PORTA) VALUES
	(666999555, 'for_textil@mail.pt','Textil suply', 223415478, 22485497, 'Fornecimento de panos e linho para produçao têxtil', '1254-585', 'Rua das produções', 23),
	(478545217, 'forn_materiais@mail.pt', 'Textil material inc.', 222454485, 2135748, 'Fornecimento de materiais para produçao textil', '4558-547', 'Rua do trabalho', 20);

INSERT INTO MATERIAIS_TÊXTEIS (REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO) VALUES
	(300, 478545217, 'Azul', 'Pano linho'),
	(302, 666999555, 'Preto', 'Pano texturado de ganga'),
	(303, 666999555, 'Castanho', 'Mola com desenho infantil'),
	(301, 478545217, 'Branco', 'Linha grossa'),
	(304, 666999555, 'Amarelo', 'Elastico usualmente usado para coletes'),
	(305, 666999555, 'Preto', 'Fecho de casaco'),
	(306, 666999555, 'Castanho', 'Botão de calça'),
	(307, 666999555, 'Amarelo', 'Fita velcro para casaco e afins');

INSERT INTO PANO(REFERENCIA_FABRICA, TIPO, GRAMAGEM, AREA_ARMAZEM, PRECO_POR_M2) VALUES
	(1, 'Cetim', 220, 152.3 , 2.5),
	(2, 'Ganga', 200, 30.5 , 1.75 );

INSERT INTO LINHA(REFERENCIA_FABRICA, GROSSURA, COMPRIMENTO_ARMAZEM, PRECO_CEM_METROS) VALUES
	(4, 2, 134.5, 0.55);

INSERT INTO ACESSORIO(REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, PRECO_UNIDADE) VALUES
	(3, 4, 0.05),
	(5, 3, 0.2),
	(6, 0, 0.1),
	(7, 0, 0.8),
	(8, 0, 0.4)

INSERT INTO MOLA(REFERENCIA_FABRICA, DIAMETRO) VALUES
	(3, 1.6);

INSERT INTO ELASTICO(REFERENCIA_FABRICA, LARGURA, COMPRIMENTO) VALUES
	(5, 1, 10);

INSERT INTO FECHO(REFERENCIA_FABRICA, COMPRIMENTO, TAMANHO_DENTE) VALUES
	(6, 8, 0.2);

INSERT INTO BOTAO(REFERENCIA_FABRICA, DIAMETRO) VALUES
	(7, 0.4);

INSERT INTO [FITA-VELCRO] (REFERENCIA_FABRICA, LARGURA, COMPRIMENTO) VALUES
	(8, 1, 2.1);

INSERT INTO [MATERIAIS-PRODUTO] (REFERENCIA, TAMANHO, COR, ID, REFERENCIA_FABRICA, QUANTIDADE) VALUES
	(1, 'XL', 'azul escuro', 1 , 1 , 50),
	(1, 'XL', 'verde lima', 2 , 1 , 50),
	(2, 'M', 'azul claro', 3 , 5 , 50);

SELECT * FROM [MATERIAIS-PRODUTO]
