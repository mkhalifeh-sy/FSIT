﻿CREATE TABLE [OPERATION.USERS.ACTION]
(
	[USERS_ACTION_ID] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[USERS_ACTION_DATE] DATETIME2 NULL,
	[USERS_ACTION_TYPE] BIGINT NULL,
	[USERS_ACTION_USER_ID] BIGINT NULL,
	[USERS_ACTION_RESULT_ID] BIGINT NULL

)