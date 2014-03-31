using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Services
{
    public interface IScheduledMaintenanceService
    {
        /// <summary>
        /// Deletes a scheduled maintenance
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to delete</param>
        void DeleteScheduledMaintenance(ScheduledMaintenance maintenance);

        /// <summary>
        /// Inserts a scheduled maintenance
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to insert</param>
        /// <returns>Scheduled maintenance</returns>
        ScheduledMaintenance InsertScheduledMaintenance(ScheduledMaintenance maintenance);

        /// <summary>
        /// Saves a scheduled maintenance
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to save</param>
        /// <returns>Scheduled maintenance</returns>
        ScheduledMaintenance SaveScheduledMaintenance(ScheduledMaintenance maintenance);

        /// <summary>
        /// Merges a scheduled maintenance entity
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to merge</param>
        /// <returns>Scheduled maintenance</returns>
        ScheduledMaintenance MergeScheduledMaintenance(ScheduledMaintenance maintenance);

        /// <summary>
        /// Returns all upcoming scheduled maintenances
        /// </summary>
        /// <returns>List of all upcoming scheduled maintenances</returns>
        IQueryable<ScheduledMaintenance> GetAllUpcomingMaintenances();

        /// <summary>
        /// Returns all upcoming scheduled maintenances within a specified number of days
        /// </summary>
        /// <param name="days">Number of days in future to check for scheduled maintenances</param>
        /// <returns>List of all upcoming scheduled maintenances</returns>
        IQueryable<ScheduledMaintenance> GetAllUpcomingMaintenances(int days);

        /// <summary>
        /// Returns a scheduled maintenance by id
        /// </summary>
        /// <param name="id">Id of scheduled maintenance</param>
        /// <returns>Scheduled maintenance</returns>
        ScheduledMaintenance GetScheduledMaintenanceById(int id);
    }
}
