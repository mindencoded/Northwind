DBCC CHECKIDENT ('[SUPPLIER]', RESEED, 0);
GO
SET IDENTITY_INSERT SUPPLIER ON
INSERT INTO SUPPLIER ( [ID], [COMPANY], [LAST_NAME], [FIRST_NAME], [EMAIL_ADDRESS], [JOB_TITLE], [BUSINESS_PHONE], [HOME_PHONE], [MOBILE_PHONE], [FAX_NUMBER], [ADDRESS], [CITY], [STATE_PROVINCE], [ZIP_POSTAL_CODE], [COUNTRY_REGION], [WEB_PAGE], [NOTES], [ATTACHMENTS]) VALUES 
     (1, 'Supplier A', 'Andersen'  , 'Elizabeth A.'  , NULL, 'Sales Manager'       , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(2, 'Supplier B', 'Weiler'    , 'Cornelia'      , NULL, 'Sales Manager'       , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(3, 'Supplier C', 'Kelley'    , 'Madeleine'     , NULL, 'Sales Representative', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(4, 'Supplier D', 'Sato'      , 'Naoki'         , NULL, 'Marketing Manager'   , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(5, 'Supplier E', 'Hernandez-Echevarria','Amaya', NULL, 'Sales Manager'       , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(6, 'Supplier F', 'Hayakawa'  , 'Satomi'        , NULL, 'Marketing Assistant' , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(7, 'Supplier G', 'Glasson'   , 'Stuart'        , NULL, 'Marketing Manager'   , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(8, 'Supplier H', 'Dunton'    , 'Bryn Paul'     , NULL, 'Sales Representative', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(9, 'Supplier I', 'Sandberg'  , 'Mikael'        , NULL, 'Sales Manager'       , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
    ,(10,'Supplier J', 'Sousa'     , 'Luis'          , NULL, 'Sales Manager'       , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
SET IDENTITY_INSERT SUPPLIER OFF
