﻿ALTER TABLE [dbo].[USER]
	ADD CONSTRAINT [FK_USER__USER_TYPE]
	FOREIGN KEY (USER_TYPE_ID)
	REFERENCES [USER_TYPE] (ID)
