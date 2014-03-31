CREATE TABLE [systemsStatus].[dbo].[ss_degraded_statuses](
	[status_id] [int] NOT NULL,
	[degraded_status_cause] [nvarchar](2000) NOT NULL,
	[degraded_status_degraded_since] [datetime] NOT NULL,
	[degraded_status_exp_online] [datetime] NULL,
	[degraded_status_act_online] [datetime] NULL
)