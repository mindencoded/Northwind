﻿CREATE TABLE [dbo].[ROLE_GROUP]
(
	[ID] INT NOT NULL IDENTITY(1,1),
	[NAME] VARCHAR(50) NOT NULL,
	CONSTRAINT [PK_ROLE_GROUP] PRIMARY KEY CLUSTERED ([ID] ASC)
)
