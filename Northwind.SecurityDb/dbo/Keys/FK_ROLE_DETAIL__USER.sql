﻿ALTER TABLE [dbo].[ROLE_DETAIL]
	ADD CONSTRAINT [FK_ROLE_DETAIL__USER]
	FOREIGN KEY (USER_ID)
	REFERENCES [USER] (ID)
