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

/*
CREATE Trigger produzirProduto  ON dbo.[PRODUTO-PERSONALIZADO]
AFTER UPDATE
AS
	BEGIN
		DECLARE @qtdProdutoPrecisa as int;
		DECLARE @qtdProdutoExistente as int;
		DECLARE @validation as bit;
		set @validation = 0;
		SELECT @qtdProdutoPrecisa = inserted.UNIDADES_ARMAZEM FROM inserted;
		SELECT @qtdProdutoExistente = UNIDADES_ARMAZEM from [PRODUTO-PERSONALIZADO] where  REFERENCIA=(SELECT REFERENCIA FROM inserted) AND COR=(SELECT COR FROM inserted) AND TAMANHO=(SELECT TAMANHO FROM inserted) AND ID=(SELECT ID FROM inserted);
		IF (@qtdProdutoExistente-@qtdProdutoExistente) >= 0
			BEGIN
			DECLARE @referenciaMaterial as int;
			DECLARE @qtdPrecisa as decimal(10,2);
			DECLARE @qtdExistente as decimal(10,2);
			DECLARE cursorMaterial CURSOR LOCAL FOR 
			SELECT REFERENCIA_FABRICA FROM [PRODUTO-PERSONALIZADO] 
			JOIN [MATERIAIS-PRODUTO] ON [PRODUTO-PERSONALIZADO].REFERENCIA=[MATERIAIS-PRODUTO].REFERENCIA;
			OPEN cursorMaterial;
			FETCH cursorMaterial INTO @referenciaMaterial;
			WHILE @@FETCH_STATUS= 0
				BEGIN
					select @qtdPrecisa = [MATERIAIS-PRODUTO].QUANTIDADE FROM [MATERIAIS-PRODUTO] 
					JOIN [PRODUTO-PERSONALIZADO] ON [MATERIAIS-PRODUTO].REFERENCIA= [PRODUTO-PERSONALIZADO].REFERENCIA 
					AND [MATERIAIS-PRODUTO].COR=[PRODUTO-PERSONALIZADO].COR 
					AND [MATERIAIS-PRODUTO].TAMANHO=[PRODUTO-PERSONALIZADO].TAMANHO 
					AND [MATERIAIS-PRODUTO].ID=[PRODUTO-PERSONALIZADO].ID
					WHERE [MATERIAIS-PRODUTO].REFERENCIA_FABRICA=@referenciaMaterial;
						BEGIN
						select @qtdPrecisa*=(@qtdProdutoPrecisa-@qtdProdutoExistente) FROM inserted;
						--validar primeiro se temos o numero necessario de materiais em stock
						IF dbo.getTipoMaterial(@referenciaMaterial) = 'Pano'
							BEGIN
								SELECT @qtdExistente = PANO.AREA_ARMAZEM FROM PANO WHERE REFERENCIA_FABRICA=@referenciaMaterial
								IF(@qtdExistente < @qtdPrecisa)
									BEGIN
										--RAISERROR ('Não existe pano suficiente para satisfazer as necessidades de produção do produto selecionado', 10, 1);
										SET @validation = 1;
									END
							END

						ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Linha'
							BEGIN
								SELECT @qtdExistente = LINHA.COMPRIMENTO_ARMAZEM FROM LINHA WHERE REFERENCIA_FABRICA=@referenciaMaterial
								IF(@qtdExistente-@qtdPrecisa<0)
									BEGIN
										--RAISERROR ('Não existe linha suficiente para satisfazer as necessidades de produção do produto selecionado', 10, 1);
										SET @validation = 1;
									END
							END
						
						ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Acessório'
							BEGIN
							SELECT @qtdExistente = ACESSORIO.QUANTIDADE_ARMAZEM FROM ACESSORIO WHERE REFERENCIA_FABRICA=@referenciaMaterial
							IF(@qtdExistente-@qtdPrecisa<0)
								BEGIN
									--RAISERROR ('Não existem acessórios suficientes para satisfazer as necessidades de produção do produto selecionado', 10, 1);
									SET @validation = 1;
								END
							END		
					FETCH cursorMaterial INTO @referenciaMaterial;
					END
				CLOSE cursorMaterial;

				OPEN cursorMaterial;
				FETCH cursorMaterial INTO @referenciaMaterial;
				----se tivermos todos os materiais necessários, remover a quantidade necessária de material
				--IF (@validation = 0)
				--BEGIN
				--	WHILE @@FETCH_STATUS = 0
				--		BEGIN
				--			select @qtdPrecisa = [MATERIAIS-PRODUTO].QUANTIDADE FROM [MATERIAIS-PRODUTO] 
				--			JOIN [PRODUTO-PERSONALIZADO] ON [MATERIAIS-PRODUTO].REFERENCIA= [PRODUTO-PERSONALIZADO].REFERENCIA 
				--			AND [MATERIAIS-PRODUTO].COR=[PRODUTO-PERSONALIZADO].COR 
				--			AND [MATERIAIS-PRODUTO].TAMANHO=[PRODUTO-PERSONALIZADO].TAMANHO 
				--			AND [MATERIAIS-PRODUTO].ID=[PRODUTO-PERSONALIZADO].ID
				--			WHERE [MATERIAIS-PRODUTO].REFERENCIA_FABRICA=@referenciaMaterial;
				--			select @qtdPrecisa*=(@qtdProdutoPrecisa-@qtdProdutoExistente) FROM inserted;
							
				--			IF dbo.getTipoMaterial(@referenciaMaterial) = 'Pano'
				--				BEGIN
				--					UPDATE [PRODUTO-PERSONALIZADO]
				--					SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
				--					FROM inserted;
				--					UPDATE PANO
				--					SET AREA_ARMAZEM=@qtdExistente-@qtdPrecisa
				--					WHERE REFERENCIA_FABRICA=@referenciaMaterial
				--				END		
				--				ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Linha'
				--						BEGIN
				--							UPDATE [PRODUTO-PERSONALIZADO]
				--							SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
				--							FROM inserted;
				--							UPDATE LINHA
				--							SET COMPRIMENTO_ARMAZEM=@qtdExistente-@qtdPrecisa
				--							WHERE REFERENCIA_FABRICA=@referenciaMaterial;
				--						END
							
				--				ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Acessório'
				--					BEGIN
				--						UPDATE [PRODUTO-PERSONALIZADO]
				--						SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
				--						FROM inserted;
				--						UPDATE ACESSORIO
				--						SET QUANTIDADE_ARMAZEM=@qtdExistente-@qtdPrecisa
				--						WHERE REFERENCIA_FABRICA=@referenciaMaterial;
				--						END
				--					END
				--			FETCH cursorMaterial INTO @referenciaMaterial;
				--		END;
				--	CLOSE cursorMaterial;
				END
				DEALLOCATE cursorMaterial;
	END
	END
GO

--drop trigger produzirProduto
*/
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
		DECLARE @n INT
		SELECT @n = [FABRICA-FILIAL].N_FILIAL FROM [FABRICA-FILIAL] 
		WHERE EMAIL = @email AND TELEFONE = @telefone
		return @n
	END
GO



/*
SELECT dbo.nUnidadesProd(1, 'XL', 'azul escuro', 1);
SELECT * FROM PANO

UPDATE [PRODUTO-PERSONALIZADO]
SET UNIDADES_ARMAZEM = 2
WHERE REFERENCIA = 1 AND TAMANHO = 'XL' AND COR = 'azul escuro' AND ID = 1;
*/
