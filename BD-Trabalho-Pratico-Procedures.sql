USE [GESTAO-FABRICA-VESTUARIO-LABORAL];
GO

CREATE PROC dbo.produzirProduto (@REFERENCIA INT, @TAMANHO VARCHAR(5), @ID INT, @qtdProdutoPrecisa INT, @validation INT OUTPUT)
AS
	BEGIN
	BEGIN TRAN
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
		COMMIT TRAN
	END
GO

--BEGIN TRAN;
--DECLARE @validation as INT;
--EXEC dbo.produzirProduto 4, 'XL', 1 , 1 , @validation OUTPUT;
--SELECT @validation;
--COMMIT TRAN;


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


CREATE PROCEDURE registarProdutoPersonalizado(@ref int, @tamanho varchar(5), @cor varchar(15), @nEtiqueta int, @preco decimal(7,2))
AS
	BEGIN
	BEGIN TRAN
		DECLARE @ID INT;
		SELECT @ID = COUNT(*)+1 FROM [PRODUTO-PERSONALIZADO] join [PRODUTO-PERSONALIZADO-DETALHES] ON [PRODUTO-PERSONALIZADO].ID=[PRODUTO-PERSONALIZADO-DETALHES].ID AND [PRODUTO-PERSONALIZADO].REFERENCIA=[PRODUTO-PERSONALIZADO-DETALHES].REFERENCIA 
		WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@ref;
		INSERT INTO [PRODUTO-PERSONALIZADO](REFERENCIA, ID, TAMANHO, PRECO, UNIDADES_ARMAZEM)
		VALUES(@ref, @ID, @tamanho, @preco, 0);
		INSERT INTO [PRODUTO-PERSONALIZADO-DETALHES](REFERENCIA, ID, COR, N_ETIQUETA)
		VALUES(@ref, @ID, @cor, @nEtiqueta);
	COMMIT TRAN
	END
GO

CREATE PROC dbo.usarMaterial(@referenciaMaterial INT, @qtdPrecisa DECIMAL(10,2))
AS
BEGIN
BEGIN TRAN
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
COMMIT TRAN
END
GO

CREATE PROC dbo.entregarEncomenda (@NENCOMENDA INT, @OUT VARCHAR(70) OUTPUT)
as 
begin
		DECLARE @QTDPRECISA INT;
		DECLARE @QTDEXISTENTE INT;
		DECLARE @ENTRGAENCOMENDA BIT;
		SET @ENTRGAENCOMENDA=1;
		DECLARE @REFERENCIA INT;
		DECLARE @TAMANHO VARCHAR(5);
		DECLARE @ID INT;
		DECLARE @ESTADO INT;
		SELECT @ESTADO=ENCOMENDA.ESTADO FROM ENCOMENDA WHERE N_ENCOMENDA=@NENCOMENDA;
		IF(@ESTADO = 2)
			BEGIN
				SET @OUT = 'A encomenda já foi entregue!';
				RETURN;
			END		
		ELSE IF (@ESTADO = 3)
			BEGIN
				SET @OUT = 'A encomenda foi cancelada. Não pode ser entregue!';
				RETURN;
			END		
		DECLARE cursorProduto CURSOR LOCAL FOR 
				SELECT CONTEUDO_ENCOMENDA.QUANTIDADE, [PRODUTO-PERSONALIZADO].UNIDADES_ARMAZEM, [PRODUTO-PERSONALIZADO].REFERENCIA, [PRODUTO-PERSONALIZADO].TAMANHO, [PRODUTO-PERSONALIZADO].ID FROM CONTEUDO_ENCOMENDA 
				JOIN [PRODUTO-PERSONALIZADO] ON [PRODUTO-PERSONALIZADO].REFERENCIA=CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO 
				AND [PRODUTO-PERSONALIZADO].TAMANHO=CONTEUDO_ENCOMENDA.TAMANHO_PRODUTO 
				AND [PRODUTO-PERSONALIZADO].ID=CONTEUDO_ENCOMENDA.ID_PRODUTO
				WHERE CONTEUDO_ENCOMENDA.N_ENCOMENDA=@NENCOMENDA;
				OPEN cursorProduto;
				FETCH cursorProduto INTO @QTDPRECISA, @QTDEXISTENTE, @REFERENCIA, @TAMANHO, @ID;
				WHILE @@FETCH_STATUS= 0
				BEGIN
					IF @QTDEXISTENTE<@QTDPRECISA
						SET @ENTRGAENCOMENDA=0;
					FETCH cursorProduto INTO @QTDPRECISA, @QTDEXISTENTE, @REFERENCIA, @TAMANHO, @ID;
				END

				IF @ENTRGAENCOMENDA=0
					BEGIN	
						SET @OUT = 'Não existem produtos suficientes para entregar a encomenda';
					END
				ELSE
					BEGIN
						CLOSE cursorProduto;
						OPEN cursorProduto;
						FETCH cursorProduto INTO @QTDPRECISA, @QTDEXISTENTE, @REFERENCIA, @TAMANHO, @ID;
						WHILE @@FETCH_STATUS= 0
						BEGIN
							UPDATE [PRODUTO-PERSONALIZADO] 
							SET UNIDADES_ARMAZEM=UNIDADES_ARMAZEM-@QTDPRECISA 
							WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA 
							AND [PRODUTO-PERSONALIZADO].TAMANHO=@TAMANHO 
							AND [PRODUTO-PERSONALIZADO].ID=@ID;
							FETCH cursorProduto INTO @QTDPRECISA, @QTDEXISTENTE, @REFERENCIA, @TAMANHO, @ID;
						END
						UPDATE ENCOMENDA SET ESTADO=2, DATA_ENTREGA=CAST(GETDATE() AS DATE) WHERE ENCOMENDA.N_ENCOMENDA=@NENCOMENDA
						SET @OUT = 'Encomenda Entregue';
					END
				CLOSE cursorProduto;
		END
go

CREATE PROC dbo.cancelarEncomenda (@NENCOMENDA INT, @OUT VARCHAR(70) OUTPUT)
as 
	BEGIN
		BEGIN TRAN
		DECLARE @ESTADO INT;
		SELECT @ESTADO=ENCOMENDA.ESTADO FROM ENCOMENDA WHERE N_ENCOMENDA=@NENCOMENDA;
		IF(@ESTADO = 3)
			BEGIN
				SET @OUT = 'A encomenda já foi cancelada!';
				RETURN
			END		
		ELSE IF (@ESTADO =2)
			BEGIN
				SET @OUT = 'Uma encomenda já entregue não pode ser cancelada!';
				RETURN
			END	
		ELSE 
			BEGIN
				UPDATE ENCOMENDA SET ESTADO=3 WHERE N_ENCOMENDA=@NENCOMENDA;
				SET @OUT = 'A encomenda foi cancelada com sucesso.';
			END		
		COMMIT TRAN
	END
go
