CREATE TABLE [systemsStatus].[dbo].[ss_statuses](
	[status_id] [int] IDENTITY(1,1) NOT NULL,
	[status_name] [nvarchar](200) NOT NULL,
	[status_created_by] [int] NOT NULL,
	[status_created_date] [datetime] NOT NULL,
	[status_type] [nvarchar](50) NOT NULL
)