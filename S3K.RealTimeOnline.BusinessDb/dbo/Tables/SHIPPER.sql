﻿CREATE TABLE [dbo].[SHIPPER]
(
	[ID]              INT NOT NULL IDENTITY,
	[COMPANY]         VARCHAR(50) ,
	[LAST_NAME]       VARCHAR(50) ,
	[FIRST_NAME]      VARCHAR(50) ,
	[EMAIL_ADDRESS]   VARCHAR(50) ,
	[JOB_TITLE]       VARCHAR(50) ,
	[BUSINESS_PHONE]  VARCHAR(25) ,
	[HOME_PHONE]      VARCHAR(25) ,
	[MOBILE_PHONE]    VARCHAR(25) ,
	[FAX_NUMBER]      VARCHAR(25) ,
	[ADDRESS]         VARCHAR(250) ,
	[CITY]            VARCHAR(50) ,
	[STATE_PROVINCE]  VARCHAR(50) ,
	[ZIP_POSTAL_CODE] VARCHAR(15) ,
	[COUNTRY_REGION]  VARCHAR(50) ,
	[WEB_PAGE]        VARCHAR(250) ,
	[NOTES]           VARCHAR(250) ,
	[ATTACHMENTS]     VARBINARY(MAX),
	CONSTRAINT [PK_SHIPPER] PRIMARY KEY CLUSTERED ([ID] ASC)
)
