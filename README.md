Systems Status
=============

SUNY Cortland System Status Application

Install
=============

1.) Run all SQL files for either Oracle or SQL Server installation.

2.) Open project in Visual Studio and download all Nuget dependencies.

3.) Set build action to 'Compile' for all SQL mappings in appropriate namespace:

        Data.Repositories.Nh.Mappings.MsSql
		Data.Repositories.Nh.Mappings.Oracle
		
4.) Set DB configuration in Dependency.PersistenceFacility.cs for appropriate database:
	
		Change line 11:
			using SystemsStatus.Data.Repositories.Nh.Mappings.Oracle;
			using SystemsStatus.Data.Repositories.Nh.Mappings.MsSql;
		
		Change line 36:
			.Database(NhOracleConfiguration.Dialect.ConnectionString(x => x.FromConnectionStringWithKey("DBConnect")))
			.Database(NhMsSql2012Configuration.Dialect.ConnectionString(x => x.FromConnectionStringWithKey("DBConnect")))
			
5.) Web.Config
		
		Add connection strings
		Add CasHost key
		
6.) Run locally or set up application in IIS.

7.) Navigate to /Admin/Install to set up initial administrator account.