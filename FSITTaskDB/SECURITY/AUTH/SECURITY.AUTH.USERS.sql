﻿CREATE TABLE [SECURITY.AUTH.USERS]
(
	[USER_ID] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[USER_EMAIL] NVARCHAR(100) NULL,
	[USER_PASSSWORD] NVARCHAR(2048) NULL,
	[USER_FIRST_NAME] NVARCHAR(100) NULL,
	[USER_LAST_NAME] NVARCHAR(100) NULL,
	[USER_MOBILE] NVARCHAR(50) NULL,
)
