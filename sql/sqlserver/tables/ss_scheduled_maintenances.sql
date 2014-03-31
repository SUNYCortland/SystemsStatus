CREATE TABLE [systemsStatus].[dbo].[ss_scheduled_maintenances](
	[maintenance_id] [int] IDENTITY(1,1) NOT NULL,
	[maintenance_name] [nvarchar](200) NOT NULL,
	[maintenance_description] [nvarchar](2000) NOT NULL,
	[maintenance_service_id] [int] NULL,
	[maintenance_offline] [datetime] NOT NULL,
	[maintenance_expected_online] [datetime] NULL,
	[maintenance_online] [datetime] NULL,
	[maintenance_scheduled_by] [int] NOT NULL,
	PRIMARY KEY([maintenance_id])
)