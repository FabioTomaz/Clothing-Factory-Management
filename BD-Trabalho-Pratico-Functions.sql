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

CREATE FUNCTION dbo.getMateriaisFromProdutoPersonalizado(@REF INT, @TAM VARCHAR(5), @ID INT) RETURNS TABLE
AS
RETURN	SELECT [MATERIAIS-PRODUTO].QUANTIDADE,  MATERIAIS_TÊXTEIS.COR, MATERIAIS_TÊXTEIS.DESIGNACAO, MATERIAIS_TÊXTEIS.NIF_FORNECEDOR, MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA, MATERIAIS_TÊXTEIS.REFERENCIA_FORN FROM [MATERIAIS-PRODUTO] 
		JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA=[MATERIAIS-PRODUTO].REFERENCIA_FABRICA 
		WHERE [MATERIAIS-PRODUTO].REFERENCIA=@REF AND [MATERIAIS-PRODUTO].TAMANHO=TAMANHO AND [MATERIAIS-PRODUTO].ID=@ID
GO


CREATE FUNCTION dbo.getQuantidadeMaterial(@referenciaMaterial INT) RETURNS DECIMAL(10,2)
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

CREATE FUNCTION dbo.checkIfProdutoPersonalizadoExits(@REFERENCIA INT, @TAMANHO VARCHAR(5), @ID INT) RETURNS BIT
AS 
	BEGIN
		DECLARE @n INT;
		SELECT @n=COUNT(*) FROM [PRODUTO-PERSONALIZADO] WHERE REFERENCIA=@REFERENCIA AND ID=@ID AND TAMANHO=@TAMANHO;
		DECLARE @exists BIT;
		IF @n>0
			SET @exists=1;
		ELSE
			SET @exists=0;
		RETURN @exists;
	END
GO

CREATE PROC dbo.produzirProduto (@REFERENCIA INT, @TAMANHO VARCHAR(5), @ID INT, @qtdProdutoPrecisa INT, @validation INT OUTPUT)
AS
	BEGIN
		SET @validation = dbo.checkIfProdutoPersonalizadoExits(@REFERENCIA, @TAMANHO, @ID);
		print @validation;
		IF @validation=0
			RETURN;
		DECLARE @qtdProdutoExistente as int;
		SELECT @qtdProdutoExistente = UNIDADES_ARMAZEM from [PRODUTO-PERSONALIZADO] 
		WHERE REFERENCIA=@REFERENCIA AND TAMANHO=@TAMANHO AND ID=@ID;
		IF (@qtdProdutoExistente-@qtdProdutoExistente) >= 0
			BEGIN
				DECLARE @referenciaMaterial as int;
				DECLARE @qtdPrecisa as decimal(10,2);
				DECLARE @qtdExistente as decimal(10,2);
				DECLARE cursorMaterial CURSOR FOR 
				SELECT REFERENCIA_FABRICA, QUANTIDADE FROM [PRODUTO-PERSONALIZADO] 
				JOIN [MATERIAIS-PRODUTO] ON 
				[PRODUTO-PERSONALIZADO].REFERENCIA=[MATERIAIS-PRODUTO].REFERENCIA
				AND [PRODUTO-PERSONALIZADO].ID=[MATERIAIS-PRODUTO].ID 
				AND [PRODUTO-PERSONALIZADO].TAMANHO=[MATERIAIS-PRODUTO].TAMANHO
				WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA
				AND [PRODUTO-PERSONALIZADO].TAMANHO = @TAMANHO 
				AND [PRODUTO-PERSONALIZADO].ID = @ID;
				OPEN cursorMaterial;
				FETCH cursorMaterial INTO @referenciaMaterial, @qtdPrecisa;
				WHILE @@FETCH_STATUS= 0
				BEGIN
					IF((dbo.getQuantidadeMaterial(@referenciaMaterial)-(@qtdPrecisa*@qtdProdutoPrecisa))<0)
						BEGIN
							SET @validation = 0;
						END
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
							EXEC dbo.usarMaterial @referenciaMaterial, @qtdPrecisa;
							FETCH cursorMaterial INTO @referenciaMaterial , @qtdPrecisa;
						END
					CLOSE cursorMaterial;
					UPDATE [PRODUTO-PERSONALIZADO]
					SET UNIDADES_ARMAZEM=@qtdProdutoExistente+@qtdProdutoPrecisa
					WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA 
					AND [PRODUTO-PERSONALIZADO].TAMANHO=@TAMANHO 
					AND [PRODUTO-PERSONALIZADO].ID=@ID; 
				END
				DEALLOCATE cursorMaterial;
		END
	END
GO

BEGIN TRAN;
DECLARE @validation as INT;
EXEC dbo.produzirProduto 4, 'XL', 1 , 1 , @validation OUTPUT;
SELECT @validation;
COMMIT TRAN;


CREATE FUNCTION dbo.checkIfCodPostalExists(@codPostal1 int, @codPostal2 int) returns bit
as
begin
	DECLARE @exists bit;
	select @exists=count(*) from ZONA WHERE CODPOSTAL1=@codPostal1 AND CODPOSTAL2=@codPostal2
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

CREATE FUNCTION dbo.getProductMaterials (@referencia int, @tamanho varchar(5), @id int) RETURNS Table
AS
	Return(
		SELECT MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA, REFERENCIA_FORN, NIF_FORNECEDOR, MATERIAIS_TÊXTEIS.COR, DESIGNACAO, QUANTIDADE 
		FROM MATERIAIS_TÊXTEIS JOIN [MATERIAIS-PRODUTO] 
		ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = [MATERIAIS-PRODUTO].REFERENCIA_FABRICA
		WHERE REFERENCIA = @referencia AND TAMANHO = @tamanho AND ID = @id
		)
GO

CREATE FUNCTION dbo.nUnidadesProd (@referencia int, @tamanho varchar(5),  @id int ) RETURNS INT
AS
	BEGIN
		DECLARE @nUnidades INT
		SELECT @nUnidades = UNIDADES_ARMAZEM FROM [PRODUTO-PERSONALIZADO] 
		WHERE REFERENCIA = @referencia AND TAMANHO = @tamanho AND ID = @id
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
		BEGIN TRAN;
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
		COMMIT TRAN;
	END
GO

--SELECT * FROM UTILIZADOR JOIN ZONA ON ( UTILIZADOR.CODPOSTAL1 = ZONA.CODPOSTAL1 AND UTILIZADOR.CODPOSTAL2 = ZONA.CODPOSTAL2)
--WHERE N_FUNCIONARIO =1

CREATE FUNCTION dbo.checkIfModeloExists(@ref int, @modelo INT) RETURNS BIT
AS
	BEGIN
		BEGIN TRAN;
		DECLARE @n INT;
		SELECT @n=COUNT(*) FROM [PRODUTO-PERSONALIZADO] WHERE REFERENCIA=@ref AND ID=@modelo;
		DECLARE @exists BIT;
		IF @n>0
			SET @exists=1;
		ELSE
			SET @exists=0;
		COMMIT TRAN;
		RETURN @exists;
	END
GO

CREATE PROCEDURE registarProdutoPersonalizado(@ref int, )