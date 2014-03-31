CREATE TABLE [systemsStatus].[dbo].[ss_unplanned_statuses](
	[status_id] [int] NOT NULL,
	[unplanned_status_offline_since] [datetime] NULL,
	[unplanned_status_exp_online] [datetime] NULL,
	[unplanned_status_act_online] [datetime] NULL,
	[unplanned_status_offline_cause] [nvarchar](max) NULL
)