USE [GESTAO-FABRICA-VESTUARIO-LABORAL];

CREATE TRIGGER updateEncomenda ON dbo.Encomenda
after update
as
	declare @nEncomenda int;
	declare @estado int;
	select @nEncomenda = N_ENCOMENDA from inserted;
	select @estado = ESTADO from inserted;
	if @estado != 1
		ROLLBACK TRAN;
go