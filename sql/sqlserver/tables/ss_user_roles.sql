CREATE TABLE [systemsStatus].[dbo].[ss_user_roles](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [nvarchar](200) NOT NULL,
	[role_level] [int] NOT NULL,
	PRIMARY KEY([role_id])
)