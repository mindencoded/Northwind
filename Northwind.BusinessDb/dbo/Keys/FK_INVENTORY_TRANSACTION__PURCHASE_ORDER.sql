﻿ALTER TABLE [dbo].[INVENTORY_TRANSACTION]
ADD CONSTRAINT [FK_INVENTORY_TRANSACTION__PURCHASE_ORDER]
FOREIGN KEY (PURCHASE_ORDER_ID)
REFERENCES [PURCHASE_ORDER] (ID)
