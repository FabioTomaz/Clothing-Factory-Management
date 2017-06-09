USE [GESTAO-FABRICA-VESTUARIO-LABORAL];
GO

--CREATE TRIGGER updateEncomenda ON dbo.Encomenda
--AFTER UPDATE
--AS
--	declare @nEncomenda int;
--	declare @estado int;
--	select @nEncomenda = N_ENCOMENDA from inserted;
--	select @estado = ESTADO from deleted WHERE N_ENCOMENDA=@nEncomenda;
--	print @estado;
--	if @estado != 1
--		ROLLBACK TRAN;
--GO


