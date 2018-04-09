﻿DBCC CHECKIDENT ('[ROLE]', RESEED, 0);
GO
SET IDENTITY_INSERT [dbo].[ROLE] ON 
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (1, N'CustomerCrud.Select', 1);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (2, N'CustomerCrud.Insert', 1);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (3, N'CustomerCrud.Update', 1);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (4, N'CustomerCrud.Delete', 1);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (5, N'EmployeeCrud.Select', 2);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (6, N'EmployeeCrud.Insert', 2);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (7, N'EmployeeCrud.Update', 2);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (8, N'EmployeeCrud.Delete', 2);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (9, N'InventoryTransactionCrud.Select', 3);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (10, N'InventoryTransactionCrud.Insert', 3);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (11, N'InventoryTransactionCrud.Update', 3);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (12, N'InventoryTransactionCrud.Delete', 3);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (13, N'InventoryTransactionTypeCrud.Select', 4);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (14, N'InventoryTransactionTypeCrud.Insert', 4);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (15, N'InventoryTransactionTypeCrud.Update', 4);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (16, N'InventoryTransactionTypeCrud.Delete', 4);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (17, N'InvoiceCrud.Select', 5);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (18, N'InvoiceCrud.Insert', 5);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (19, N'InvoiceCrud.Update', 5);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (20, N'InvoiceCrud.Delete', 5);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (21, N'OrderCrud.Select', 6);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (22, N'OrderCrud.Insert', 6);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (23, N'OrderCrud.Update', 6);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (24, N'OrderCrud.Delete', 6);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (25, N'OrderDetailCrud.Select', 7);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (26, N'OrderDetailCrud.Insert', 7);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (27, N'OrderDetailCrud.Update', 7);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (28, N'OrderDetailCrud.Delete', 7);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (29, N'OrderDetailStatusCrud.Select', 8);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (30, N'OrderDetailStatusCrud.Insert', 8);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (31, N'OrderDetailStatusCrud.Update', 8);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (32, N'OrderDetailStatusCrud.Delete', 8);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (33, N'OrderStatusCrud.Select', 9);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (34, N'OrderStatusCrud.Insert', 9);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (35, N'OrderStatusCrud.Update', 9);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (36, N'OrderStatusCrud.Delete', 9);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (37, N'OrderTaxStatusCrud.Select', 10);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (38, N'OrderTaxStatusCrud.Insert', 10);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (39, N'OrderTaxStatusCrud.Update', 10);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (40, N'OrderTaxStatusCrud.Delete', 10);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (41, N'ProductCrud.Select', 11);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (42, N'ProductCrud.Insert', 11);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (43, N'ProductCrud.Update', 11);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (44, N'ProductCrud.Delete', 11);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (45, N'PurchaseOrderCrud.Select', 12);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (46, N'PurchaseOrderCrud.Insert', 12);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (47, N'PurchaseOrderCrud.Update', 12);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (48, N'PurchaseOrderCrud.Delete', 12);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (49, N'PurchaseOrderDetailCrud.Select', 13);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (50, N'PurchaseOrderDetailCrud.Insert', 13);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (51, N'PurchaseOrderDetailCrud.Update', 13);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (52, N'PurchaseOrderDetailCrud.Delete', 13);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (53, N'PurchaseOrderStatusCrud.Select', 14);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (54, N'PurchaseOrderStatusCrud.Insert', 14);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (55, N'PurchaseOrderStatusCrud.Update', 14);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (56, N'PurchaseOrderStatusCrud.Delete', 14);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (57, N'RoleCrud.Select', 15);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (58, N'RoleCrud.Insert', 15);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (59, N'RoleCrud.Update', 15);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (60, N'RoleCrud.Delete', 15);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (61, N'RoleDetailCrud.Select', 16);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (62, N'RoleDetailCrud.Insert', 16);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (63, N'RoleDetailCrud.Update', 16);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (64, N'RoleDetailCrud.Delete', 16);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (65, N'RoleGroupCrud.Select', 17);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (66, N'RoleGroupCrud.Insert', 17);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (67, N'RoleGroupCrud.Update', 17);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (68, N'RoleGroupCrud.Delete', 17);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (69, N'ShipperCrud.Select', 18);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (70, N'ShipperCrud.Insert', 18);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (71, N'ShipperCrud.Update', 18);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (72, N'ShipperCrud.Delete', 18);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (73, N'SupplierCrud.Select', 19);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (74, N'SupplierCrud.Insert', 19);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (75, N'SupplierCrud.Update', 19);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (76, N'SupplierCrud.Delete', 19);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (77, N'UserCrud.Select', 20);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (78, N'UserCrud.Insert', 20);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (79, N'UserCrud.Update', 20);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (80, N'UserCrud.Delete', 20);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (81, N'UserTypeCrud.Select', 21);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (82, N'UserTypeCrud.Insert', 21);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (83, N'UserTypeCrud.Update', 21);
INSERT [dbo].[ROLE] ([ID], [NAME], [ROLE_GROUP_ID]) VALUES (84, N'UserTypeCrud.Delete', 21);
SET IDENTITY_INSERT [dbo].[ROLE] OFF
