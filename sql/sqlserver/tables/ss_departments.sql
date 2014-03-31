CREATE TABLE [systemsStatus].[dbo].[ss_departments](
	[department_id] [int] IDENTITY(1,1) NOT NULL,
	[department_name] [nvarchar](200) NOT NULL,
	[department_code] [nvarchar](4) NOT NULL,
	PRIMARY KEY([department_id])
)