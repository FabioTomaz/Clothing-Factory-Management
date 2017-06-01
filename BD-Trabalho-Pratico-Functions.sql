USE [GESTAO-FABRICA-VESTUARIO-LABORAL]
GO

CREATE FUNCTION dbo.getTipoMaterial (@ref int) RETURNS varchar(20)
AS
	BEGIN
		DECLARE @tipo varchar(20);
		SET @tipo = 'Material Textil';
		SELECT @tipo='Pano' 
		FROM PANO
		WHERE PANO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Linha' 
		FROM LINHA
		WHERE LINHA.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Acessório' 
		FROM ACESSORIO
		WHERE ACESSORIO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Fecho' 
		FROM FECHO
		WHERE FECHO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Mola' 
		FROM MOLA
		WHERE MOLA.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Botão' 
		FROM BOTAO
		WHERE BOTAO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Elástico'
		FROM ELASTICO
		WHERE ELASTICO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Fita de Velcro'
		FROM [FITA-VELCRO]
		WHERE [FITA-VELCRO].REFERENCIA_FABRICA=@ref
		RETURN @tipo
	END
GO

CREATE FUNCTION dbo.getMateriaisFromProdutoPersonalizado(@REF INT, @TAM VARCHAR(5), @COR VARCHAR(15), @ID INT) RETURNS TABLE
AS
RETURN	SELECT [MATERIAIS-PRODUTO].QUANTIDADE,  MATERIAIS_TÊXTEIS.COR, MATERIAIS_TÊXTEIS.DESIGNACAO, MATERIAIS_TÊXTEIS.NIF_FORNECEDOR, MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA, MATERIAIS_TÊXTEIS.REFERENCIA_FORN FROM [MATERIAIS-PRODUTO] 
		JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA=[MATERIAIS-PRODUTO].REFERENCIA_FABRICA 
		WHERE [MATERIAIS-PRODUTO].REFERENCIA=@REF AND [MATERIAIS-PRODUTO].TAMANHO=TAMANHO AND [MATERIAIS-PRODUTO].COR=@COR AND [MATERIAIS-PRODUTO].ID=@ID
GO
select dbo.getTipoMaterial(1);

CREATE FUNCTION getQuantidadeMaterial(@referenciaMaterial INT) RETURNS DECIMAL(10,2)
AS 
BEGIN
	DECLARE @qtdExistente decimal(10,2);
	IF dbo.getTipoMaterial(@referenciaMaterial) = 'Pano'
		SELECT @qtdExistente = PANO.AREA_ARMAZEM FROM PANO WHERE REFERENCIA_FABRICA=@referenciaMaterial;

	ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Linha'
		SELECT @qtdExistente = LINHA.COMPRIMENTO_ARMAZEM FROM LINHA WHERE REFERENCIA_FABRICA=@referenciaMaterial;
						
	ELSE
		SELECT @qtdExistente = ACESSORIO.QUANTIDADE_ARMAZEM FROM ACESSORIO WHERE REFERENCIA_FABRICA=@referenciaMaterial;
	return @qtdExistente;
END
GO

CREATE PROC dbo.usarMaterial(@referenciaMaterial INT, @qtdPrecisa DECIMAL(10,2))
AS
BEGIN
	DECLARE @qtdExistente DECIMAL(10,2);
	SET @qtdExistente= dbo.getQuantidadeMaterial(@referenciaMaterial);
	print @qtdExistente;
	print @qtdPrecisa;
	IF dbo.getTipoMaterial(@referenciaMaterial) = 'Pano'
		BEGIN
			UPDATE PANO
			SET AREA_ARMAZEM=@qtdExistente-@qtdPrecisa
			WHERE REFERENCIA_FABRICA=@referenciaMaterial
		END		
	ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Linha'
		BEGIN
			UPDATE LINHA
			SET COMPRIMENTO_ARMAZEM=@qtdExistente-@qtdPrecisa
			WHERE REFERENCIA_FABRICA=@referenciaMaterial;
		END					
	ELSE 
		BEGIN
			UPDATE ACESSORIO
			SET QUANTIDADE_ARMAZEM=@qtdExistente-@qtdPrecisa
			WHERE REFERENCIA_FABRICA=@referenciaMaterial;
		END
END
GO

CREATE PROC dbo.produzirProduto (@REFERENCIA INT, @TAMANHO VARCHAR(5), @COR VARCHAR(15), @ID INT, @qtdProdutoPrecisa INT, @validation INT OUTPUT)
AS
	BEGIN
		SET @validation = 1;
		DECLARE @qtdProdutoExistente as int;
		SELECT @qtdProdutoExistente = UNIDADES_ARMAZEM from [PRODUTO-PERSONALIZADO] 
		where  REFERENCIA=@REFERENCIA AND COR=@COR AND TAMANHO=@TAMANHO AND ID=@ID;
		IF (@qtdProdutoExistente-@qtdProdutoExistente) >= 0
			BEGIN
				DECLARE @referenciaMaterial as int;
				DECLARE @qtdPrecisa as decimal(10,2);
				DECLARE @qtdExistente as decimal(10,2);
				DECLARE cursorMaterial CURSOR FOR 
				SELECT REFERENCIA_FABRICA, [MATERIAIS-PRODUTO].QUANTIDADE FROM [PRODUTO-PERSONALIZADO] 
				JOIN [MATERIAIS-PRODUTO] ON [PRODUTO-PERSONALIZADO].REFERENCIA=[MATERIAIS-PRODUTO].REFERENCIA
				WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA 
				AND [PRODUTO-PERSONALIZADO].COR=@COR 
				AND [PRODUTO-PERSONALIZADO].TAMANHO=@TAMANHO
				AND [PRODUTO-PERSONALIZADO].ID = @ID;
				OPEN cursorMaterial;
				FETCH cursorMaterial INTO @referenciaMaterial, @qtdPrecisa;
				WHILE @@FETCH_STATUS= 0
				BEGIN
					set @qtdPrecisa = @qtdPrecisa*@qtdProdutoPrecisa;
					IF((dbo.getQuantidadeMaterial(@referenciaMaterial)-@qtdPrecisa)<0)
						SET @validation = 0;
					FETCH cursorMaterial INTO @referenciaMaterial , @qtdPrecisa;
				END
				
				CLOSE cursorMaterial;

				IF (@validation = 1)
				BEGIN
					OPEN cursorMaterial;
					FETCH cursorMaterial INTO @referenciaMaterial, @qtdPrecisa;
					WHILE @@FETCH_STATUS = 0
						BEGIN
							SET @qtdPrecisa=@qtdPrecisa*@qtdProdutoPrecisa;
							exec dbo.usarMaterial @referenciaMaterial, @qtdPrecisa;
							FETCH cursorMaterial INTO @referenciaMaterial , @qtdPrecisa;
						END
					CLOSE cursorMaterial;
					UPDATE [PRODUTO-PERSONALIZADO]
					SET UNIDADES_ARMAZEM=@qtdProdutoExistente+@qtdProdutoPrecisa
					WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA 
					AND [PRODUTO-PERSONALIZADO].TAMANHO=@TAMANHO 
					AND [PRODUTO-PERSONALIZADO].COR=@COR 
					AND [PRODUTO-PERSONALIZADO].ID=@ID; 
				END
				DEALLOCATE cursorMaterial;
		END
	END
GO

DECLARE @validation as INT;
EXEC dbo.produzirProduto 2, 'M', 'azul claro', 3 , 1 , @validation OUTPUT;
SELECT @validation
CREATE FUNCTION dbo.checkIfCodPostalExists(@codPostal1 int, @codPostal2 int) returns bit
as
begin
	DECLARE @exists bit;
	select @exists=count(*) from ZONA WHERE CODPOSTAL1=@codPostal1 AND CODPOSTAL2=@codPostal2
	return @exists;
end
go

CREATE FUNCTION dbo.checkIfFornecedorExists ON CLIENTE
as
begin
	DECLARE @exists bit;
	select @exists=count(*) from CLIENTE WHERE CODPOSTAL1=@codPostal1 AND CODPOSTAL2=@codPostal2
	return @exists;
end
go

--DECLARE @OUT VARCHAR(100);
--EXEC dbo.produzirProduto 2, 'M', 'azul claro', 3 , 1 , @OUT

--drop proc dbo.produzirProduto

--SELECT * FROM ELASTICO JOIN ACESSORIO ON ACESSORIO.REFERENCIA_FABRICA = ELASTICO.REFERENCIA_FABRICA
--SELECT * FROM [PRODUTO-PERSONALIZADO]

GO
CREATE FUNCTION dbo.getEtiqueta (@n int) RETURNS TABLE
AS
	RETURN (SELECT * FROM ETIQUETA WHERE N_ETIQUETA = @n)
GO

CREATE FUNCTION dbo.getEtiquetaNumero (@norma varchar(100), @comp varchar(100), @pais varchar(20) ) RETURNS INT
AS
	BEGIN
		DECLARE @nEtiqueta INT
		SELECT @nEtiqueta = N_ETIQUETA FROM ETIQUETA WHERE NORMAS = @norma AND PAIS_FABRICO = @pais AND COMPOSICAO = @comp
		return @nEtiqueta
	END
GO

CREATE FUNCTION dbo.getProductMaterials (@referencia int, @tamanho varchar(5), @cor varchar(15), @id int) RETURNS Table
AS
	Return(
		SELECT MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA, REFERENCIA_FORN, NIF_FORNECEDOR, MATERIAIS_TÊXTEIS.COR, DESIGNACAO, QUANTIDADE 
		FROM MATERIAIS_TÊXTEIS JOIN [MATERIAIS-PRODUTO] 
		ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = [MATERIAIS-PRODUTO].REFERENCIA_FABRICA
		WHERE REFERENCIA = @referencia AND TAMANHO = @tamanho AND [MATERIAIS-PRODUTO].COR = @cor AND ID = @id
		)
GO

CREATE FUNCTION dbo.nUnidadesProd (@referencia int, @tamanho varchar(5), @cor varchar(15),  @id int ) RETURNS INT
AS
	BEGIN
		DECLARE @nUnidades INT
		SELECT @nUnidades = UNIDADES_ARMAZEM FROM [PRODUTO-PERSONALIZADO] 
		WHERE REFERENCIA = @referencia AND TAMANHO = @tamanho AND COR = @cor AND ID = @id
		return @nUnidades
	END
GO

CREATE FUNCTION dbo.nFilial (@email varchar(50), @telefone varchar(22) ) RETURNS INT
AS
	BEGIN
		DECLARE @nFilial INT
		SELECT @nFilial = N_FILIAL FROM [FABRICA-FILIAL] 
		WHERE EMAIL = @email AND TELEFONE = @telefone
		return @nFilial
	END
GO

CREATE FUNCTION dbo.getPrecoMaterial(@ref int) returns decimal(10,2)
AS
	BEGIN
		DECLARE @PRECO DECIMAL(10,2);
		IF dbo.getTipoMaterial(@ref) = 'Pano'
			SELECT @PRECO = PANO.PRECO_POR_M2 FROM PANO WHERE REFERENCIA_FABRICA=@ref;
		ELSE IF dbo.getTipoMaterial(@ref) = 'Linha'
			SELECT @PRECO = LINHA.PRECO_CEM_METROS FROM LINHA WHERE REFERENCIA_FABRICA=@ref;				
		ELSE
			SELECT @PRECO = ACESSORIO.PRECO_UNIDADE FROM ACESSORIO WHERE REFERENCIA_FABRICA=@ref;
		RETURN @PRECO;
	END
GO

CREATE PROC dbo.adicionarMaterial(@ref int, @qtd decimal(10,2), @OUT VARCHAR(70) OUTPUT)
AS
	BEGIN
		DECLARE @qtdExistente decimal(10,2);
		SELECT @qtdExistente=dbo.getQuantidadeMaterial(@ref);
		IF dbo.getTipoMaterial(@ref) = 'Pano'
			BEGIN
				SELECT @qtdExistente = PANO.AREA_ARMAZEM FROM PANO WHERE REFERENCIA_FABRICA=@ref;
				UPDATE PANO SET AREA_ARMAZEM=@qtdExistente+@qtd WHERE PANO.REFERENCIA_FABRICA=@ref;
			END
		ELSE IF dbo.getTipoMaterial(@ref) = 'Linha'
			BEGIN
				SELECT @qtdExistente = LINHA.COMPRIMENTO_ARMAZEM FROM LINHA WHERE REFERENCIA_FABRICA=@ref;				
				UPDATE LINHA SET COMPRIMENTO_ARMAZEM=@qtdExistente+@qtd WHERE LINHA.REFERENCIA_FABRICA=@ref;
			END
		ELSE
			BEGIN
				SELECT @qtdExistente = ACESSORIO.QUANTIDADE_ARMAZEM FROM ACESSORIO WHERE REFERENCIA_FABRICA=@ref;
				UPDATE ACESSORIO SET QUANTIDADE_ARMAZEM=@qtdExistente+@qtd WHERE ACESSORIO.REFERENCIA_FABRICA=@ref;
			END
			SET @OUT = 'A quantidade indicada foi adicionada com sucesso';
	END
GO

--SELECT * FROM UTILIZADOR JOIN ZONA ON ( UTILIZADOR.CODPOSTAL1 = ZONA.CODPOSTAL1 AND UTILIZADOR.CODPOSTAL2 = ZONA.CODPOSTAL2)
--WHERE N_FUNCIONARIO =1

SELECT * FROM [FABRICA-FILIAL] JOIN ZONA ON 
           ([FABRICA-FILIAL].CODPOSTAL1 = ZONA.CODPOSTAL1 AND [FABRICA-FILIAL].CODPOSTAL2 = ZONA.CODPOSTAL2)
		   JOIN UTILIZADOR ON CHEFE = N_FUNCIONARIO