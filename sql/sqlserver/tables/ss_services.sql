CREATE TABLE [systemsStatus].[dbo].[ss_services](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[service_name] [nvarchar](200) NOT NULL,
	[service_description] [nvarchar](2000) NOT NULL,
	[service_parent_service_id] [int] NULL,
	[service_current_status_id] [int] NOT NULL,
	[service_public] [char](1) NOT NULL,
	[service_friendly_url] [nvarchar](200) NOT NULL,
	[service_created_by] [int] NOT NULL,
	[service_sort_order] [int] NOT NULL,
	[service_department_id] [int] NULL,
	[service_sla] [nvarchar](max) NULL,
	[service_cost] [nvarchar](max) NULL,
	PRIMARY KEY([service_id])
)