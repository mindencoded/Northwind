﻿ALTER TABLE [dbo].[PURCHASE_ORDER]
ADD CONSTRAINT [FK_PURCHASE_ORDER__PURCHASE_ORDER_STATUS]
FOREIGN KEY (STATUS_ID)
REFERENCES [PURCHASE_ORDER_STATUS] (ID)