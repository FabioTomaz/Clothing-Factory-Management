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

GO
CREATE Trigger produzirProduto  ON dbo.[PRODUTO-PERSONALIZADO]
INSTEAD OF UPDATE
AS
	BEGIN
		DECLARE @qtdProdutoPrecisa as int;
		DECLARE @qtdProdutoExistente as int;
		SELECT @qtdProdutoPrecisa = inserted.UNIDADES_ARMAZEM FROM inserted;
		SELECT @qtdProdutoExistente = UNIDADES_ARMAZEM from [PRODUTO-PERSONALIZADO] where  REFERENCIA=(SELECT REFERENCIA FROM inserted) AND COR=(SELECT COR FROM inserted) AND TAMANHO=(SELECT TAMANHO FROM inserted) AND ID=(SELECT ID FROM inserted);
		IF (@qtdProdutoExistente-@qtdProdutoExistente) < 0
			BEGIN
				UPDATE [PRODUTO-PERSONALIZADO]
				SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
				FROM inserted
			END
		ELSE
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
					select @qtdPrecisa*=(@qtdProdutoPrecisa-@qtdProdutoExistente) FROM inserted;
						IF dbo.getTipoMaterial(@referenciaMaterial) = 'Pano'
							BEGIN
								SELECT @qtdExistente = PANO.AREA_ARMAZEM FROM PANO WHERE REFERENCIA_FABRICA=@referenciaMaterial
								IF(@qtdExistente-@qtdPrecisa<0)
									print 'N�o existe pano suficiente para satisfazer as necessidades de produ��o do produto selecionado'
								ELSE
									BEGIN
										UPDATE [PRODUTO-PERSONALIZADO]
										SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
										FROM inserted;
										UPDATE PANO
										SET AREA_ARMAZEM=@qtdExistente-@qtdPrecisa
										WHERE REFERENCIA_FABRICA=@referenciaMaterial
									END
							END
						ELSE IF dbo.getTipoMaterial(@referenciaMaterial) = 'Linha'
							BEGIN
								SELECT @qtdExistente = LINHA.COMPRIMENTO_ARMAZEM FROM LINHA WHERE REFERENCIA_FABRICA=@referenciaMaterial
								IF(@qtdExistente-@qtdPrecisa<0)
									print 'N�o existe linha suficiente para satisfazer as necessidades de produ��o do produto selecionado'
								ELSE
									BEGIN
										UPDATE [PRODUTO-PERSONALIZADO]
										SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
										FROM inserted;
										UPDATE LINHA
										SET COMPRIMENTO_ARMAZEM=@qtdExistente-@qtdPrecisa
										WHERE REFERENCIA_FABRICA=@referenciaMaterial;
									END
							END
						ELSE
							BEGIN
								SELECT @qtdExistente = ACESSORIO.QUANTIDADE_ARMAZEM FROM ACESSORIO WHERE REFERENCIA_FABRICA=@referenciaMaterial
								IF(@qtdExistente-@qtdPrecisa<0)
									print 'N�o existe acess�rios suficientes para satisfazer as necessidades de produ��o do produto selecionado'
								ELSE
									BEGIN
										UPDATE [PRODUTO-PERSONALIZADO]
										SET UNIDADES_ARMAZEM=inserted.UNIDADES_ARMAZEM, PRECO=inserted.PRECO, N_ETIQUETA=inserted.N_ETIQUETA
										FROM inserted;
										UPDATE ACESSORIO
										SET QUANTIDADE_ARMAZEM=@qtdExistente-@qtdPrecisa
										WHERE REFERENCIA_FABRICA=@referenciaMaterial;
									END
							END
						FETCH cursorMaterial INTO @referenciaMaterial;
					END
				CLOSE cursorMaterial;
				DEALLOCATE cursorMaterial;
		END
	END
GO

