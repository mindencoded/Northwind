﻿CREATE TABLE [dbo].[ORDER_DETAIL_STATUS]
(
	[ID]	TINYINT  NOT NULL,
	[STATUS_NAME]	VARCHAR(50) NOT NULL,
	CONSTRAINT [PK_ORDER_DETAIL_STATUS] PRIMARY KEY CLUSTERED ([ID] ASC)
)