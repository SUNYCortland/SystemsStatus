CREATE TABLE [systemsStatus].[dbo].[ss_users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_first_name] [nvarchar](50) NOT NULL,
	[user_last_name] [nvarchar](50) NOT NULL,
	[user_username] [nvarchar](50) NOT NULL,
	[user_hashed_password] [nvarchar](max) NOT NULL,
	[user_role_id] [int] NOT NULL,
	[user_password_salt] [varbinary](max) NULL,
	[user_active] [char](1) NOT NULL,
	PRIMARY KEY([user_id])
)