CREATE TABLE [systemsStatus].[dbo].[ss_widgets](
	[widget_id] [int] IDENTITY(1,1) NOT NULL,
	[widget_guid] [uniqueidentifier] NOT NULL,
	[widget_user_id] [int] NOT NULL,
	[widget_height] [int] NOT NULL,
	[widget_name] [nvarchar](200) NOT NULL,
	[widget_department_id] [int] NULL,
	PRIMARY KEY([widget_id])
)