DBCC CHECKIDENT ('[PURCHASE_ORDER]', RESEED, 0);
GO
SET IDENTITY_INSERT [PURCHASE_ORDER] ON
INSERT INTO [PURCHASE_ORDER] ([ID], [SUPPLIER_ID], [CREATED_BY], [SUBMITTED_DATE], [CREATION_DATE], [STATUS_ID], [EXPECTED_DATE], [SHIPPING_FEE], [TAX], [PAYMENT_DATE], [PAYMENT_AMOUNT], [PAYMENT_METHOD], [NOTES], [APPROVED_BY], [APPROVED_DATE], [SUBMITTED_BY]) VALUES 
     (90,  1, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-01-22', 2)
    ,(91,  3, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-01-22', 2)
    ,(92,  2, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-01-22', 2)
    ,(93,  5, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-01-22', 2)
    ,(94,  6, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-01-22', 2)
    ,(95,  4, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-01-22', 2)
    ,(96,  1, 5, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #30', 2, '2006-01-22', 5)
    ,(97,  2, 7, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #33', 2, '2006-01-22', 7)
    ,(98,  2, 4, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #36', 2, '2006-01-22', 4)
    ,(99,  1, 3, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #38', 2, '2006-01-22', 3)
    ,(100, 2, 9, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #39', 2, '2006-01-22', 9)
    ,(101, 1, 2, '2006-01-14', '2006-01-22', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #40', 2, '2006-01-22', 2)
    ,(102, 1, 1, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #41', 2, '2006-04-04', 1)
    ,(103, 2, 1, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #42', 2, '2006-04-04', 1)
    ,(104, 2, 1, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #45', 2, '2006-04-04', 1)
    ,(105, 5, 7, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #46', 2, '2006-04-04', 7)
    ,(106, 6, 7, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #46', 2, '2006-04-04', 7)
    ,(107, 1, 6, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #47', 2, '2006-04-04', 6)
    ,(108, 2, 4, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #48', 2, '2006-04-04', 4)
    ,(109, 2, 4, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #48', 2, '2006-04-04', 4)
    ,(110, 1, 3, '2006-03-24', '2006-03-24', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #49', 2, '2006-04-04', 3)
    ,(111, 1, 2, '2006-03-31', '2006-03-31', 2, NULL, 0, 0, NULL, 0, NULL, 'Purchase generated based on Order #56', 2, '2006-04-04', 2)
    ,(140, 6, 7, '2006-04-25', '2006-04-25', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-04-25', 2)
    ,(141, 8, 7, '2006-04-25', '2006-04-25', 2, NULL, 0, 0, NULL, 0, NULL, NULL, 2,                                    '2006-04-25', 2)
    ,(142, 8, 3, '2006-04-25', '2006-04-25', 2, NULL, 0, 0, NULL, 0, NULL      , NULL, 2,                              '2006-04-25', 2)
    ,(146, 2, 2, '2006-04-26', '2006-04-26', 1, NULL, 0, 0, NULL, 0, NULL, NULL, NULL, NULL, 2)
    ,(147, 7, 2, '2006-04-26', '2006-04-26', 1, NULL, 0, 0, NULL, 0, NULL, NULL, NULL, NULL, 2)
    ,(148, 5, 2, '2006-04-26', '2006-04-26', 1, NULL, 0, 0, NULL, 0, NULL, NULL, NULL, NULL, 2);
SET IDENTITY_INSERT [PURCHASE_ORDER] OFF