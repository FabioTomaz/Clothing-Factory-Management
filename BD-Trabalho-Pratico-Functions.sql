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
		SELECT @tipo='Acess�rio' 
		FROM ACESSORIO
		WHERE ACESSORIO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Fecho' 
		FROM FECHO
		WHERE FECHO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Mola' 
		FROM MOLA
		WHERE MOLA.REFERENCIA_FABRICA=@ref
		SELECT @tipo='Bot�o' 
		FROM BOTAO
		WHERE BOTAO.REFERENCIA_FABRICA=@ref
		SELECT @tipo='El�stico'
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
RETURN	SELECT [MATERIAIS-PRODUTO].QUANTIDADE,  MATERIAIS_T�XTEIS.COR, MATERIAIS_T�XTEIS.DESIGNACAO, MATERIAIS_T�XTEIS.NIF_FORNECEDOR, MATERIAIS_T�XTEIS.REFERENCIA_FABRICA, MATERIAIS_T�XTEIS.REFERENCIA_FORN FROM [MATERIAIS-PRODUTO] 
		JOIN MATERIAIS_T�XTEIS ON MATERIAIS_T�XTEIS.REFERENCIA_FABRICA=[MATERIAIS-PRODUTO].REFERENCIA_FABRICA 
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


CREATE FUNCTION dbo.checkIfCodPostalExists(@codPostal1 int, @codPostal2 int) returns bit
as
begin
	DECLARE @exists bit;
	select @exists=count(*) from ZONA WHERE CODPOSTAL1=@codPostal1 AND CODPOSTAL2=@codPostal2
	return @exists;
end
go

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
		SELECT MATERIAIS_T�XTEIS.REFERENCIA_FABRICA, REFERENCIA_FORN, NIF_FORNECEDOR, MATERIAIS_T�XTEIS.COR, DESIGNACAO, QUANTIDADE 
		FROM MATERIAIS_T�XTEIS JOIN [MATERIAIS-PRODUTO] 
		ON MATERIAIS_T�XTEIS.REFERENCIA_FABRICA = [MATERIAIS-PRODUTO].REFERENCIA_FABRICA
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

CREATE FUNCTION dbo.existsEqualProdutoPersonalizado(@ref int, @cor varchar(15), @nEtiqueta int) RETURNS int
AS
	BEGIN
		DECLARE @MODELO INT = -1;
		SELECT @MODELO=MAX([PRODUTO-PERSONALIZADO].ID) FROM [PRODUTO-PERSONALIZADO] 
		JOIN [PRODUTO-PERSONALIZADO-DETALHES] ON [PRODUTO-PERSONALIZADO-DETALHES].REFERENCIA=[PRODUTO-PERSONALIZADO].REFERENCIA 
		AND [PRODUTO-PERSONALIZADO-DETALHES].ID=[PRODUTO-PERSONALIZADO].ID 
		WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@ref 
		AND COR=@cor 
		AND N_ETIQUETA=@nEtiqueta;
		RETURN @MODELO;
	END
GO

CREATE FUNCTION dbo.getMaxModelo(@ref int) returns int
as
begin
	declare @max int;
	select @max=max(ID) from [PRODUTO-PERSONALIZADO] WHERE REFERENCIA=@ref;
	return @max;
end
go


 CREATE FUNCTION dbo.getTypeID (@type varchar(50) ) RETURNS INT
AS
	BEGIN
		DECLARE @ID INT
		SELECT @ID = ID FROM [TIPO-UTILIZADOR] 
		WHERE TIPO = @type
		return @ID
	END
GO



 CREATE FUNCTION dbo.getUserPass (@nFunc int ) RETURNS VARCHAR(30)
AS
	BEGIN
		DECLARE @pass VARCHAR(30)
		SELECT @pass = PASS FROM UTILIZADOR 
		WHERE N_FUNCIONARIO = @nFunc
		return @pass
	END
GO


--DROP FUNCTION dbo.existsEqualProdutoPersonalizado
--SELECT dbo.existsEqualProdutoPersonalizado(1, '#e12022',1);

--CREATE PROCEDURE registarProdutoPersonalizado(@ref int, @tamanho varchar(5), @cor varchar(15), @nEtiqueta int, @preco decimal(7,2))
--AS
--	BEGIN
--		DECLARE @ID INT = dbo.existsEqualProdutoPersonalizado(@ref, @cor, @nEtiqueta); 
--		PRINT @ID;
--		if  @ID != NULL
--			BEGIN
--				select @ID=max(ID)+1 from [PRODUTO-PERSONALIZADO] where REFERENCIA=@ref;
--				INSERT INTO [PRODUTO-PERSONALIZADO](REFERENCIA, ID, TAMANHO, PRECO, UNIDADES_ARMAZEM)
--				VALUES(@ref, @ID, @tamanho, @preco, 0);
--				INSERT INTO [PRODUTO-PERSONALIZADO-DETALHES](REFERENCIA, ID, COR, N_ETIQUETA)
--				VALUES(@ref, @ID, @cor, @nEtiqueta);
--			END
--		ELSE 
--			BEGIN
--				DECLARE @COUNT INT;
--				SELECT @COUNT = COUNT(*) FROM [PRODUTO-PERSONALIZADO] WHERE REFERENCIA=@ref AND TAMANHO=@tamanho;
--				IF @COUNT !=0
--					SELECT @ID = MAX(ID)+1 FROM [PRODUTO-PERSONALIZADO] WHERE REFERENCIA=@ref AND TAMANHO=@tamanho;
--				ELSE
--					BEGIN
--						SET @ID=1;
--						INSERT INTO [PRODUTO-PERSONALIZADO](REFERENCIA, ID, TAMANHO, PRECO, UNIDADES_ARMAZEM)
--						VALUES(@ref, @ID, @tamanho, @preco, 0);
--						INSERT INTO [PRODUTO-PERSONALIZADO-DETALHES](REFERENCIA, ID, COR, N_ETIQUETA)
--						VALUES(@ref, @ID, @cor, @nEtiqueta);
--					END

--				Print 'else'
--			END
--	END
--GO