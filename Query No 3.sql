USE [PruebaNexarte]
GO
DROP TABLE IF EXISTS [dbo].[LOG_CONSULTA_NOM_NOMINA_DEFINITIVA];
GO
CREATE TABLE [dbo].[LOG_CONSULTA_NOM_NOMINA_DEFINITIVA](
	[ID_REGISTRO] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[USUARIO] [nvarchar](30) NULL,
	[FECHA] [datetime] NULL,
	[OPERACION] [nvarchar](10) NULL,
	[DESCRIPCION_EVENTO] [nvarchar](max) NULL
)
GO