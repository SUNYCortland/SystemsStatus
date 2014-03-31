using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using SystemsStatus.Core.Data.Repositories;
using SystemsStatus.Core.Data;

namespace SystemsStatus.Core.Services.Impl
{
    public class ScheduledMaintenanceService : IScheduledMaintenanceService
    {
        private readonly IScheduledMaintenanceRepository _scheduledMaintenanceRepository;

        public ScheduledMaintenanceService(IScheduledMaintenanceRepository scheduledMaintenanceRepository)
        {
            _scheduledMaintenanceRepository = scheduledMaintenanceRepository;
        }

        /// <summary>
        /// Deletes a scheduled maintenance
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to delete</param>
        public void DeleteScheduledMaintenance(ScheduledMaintenance maintenance)
        {
            _scheduledMaintenanceRepository.Delete(maintenance);
        }

        /// <summary>
        /// Inserts a scheduled maintenance
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to insert</param>
        /// <returns>Scheduled maintenance</returns>
        [UnitOfWork]
        public ScheduledMaintenance InsertScheduledMaintenance(ScheduledMaintenance maintenance)
        {
            return _scheduledMaintenanceRepository.Insert(maintenance);
        }

        /// <summary>
        /// Saves a scheduled maintenance
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to save</param>
        /// <returns>Scheduled maintenance</returns>
        [UnitOfWork]
        public ScheduledMaintenance SaveScheduledMaintenance(ScheduledMaintenance maintenance)
        {
            return _scheduledMaintenanceRepository.SaveOrUpdate(maintenance);
        }

        /// <summary>
        /// Merges a scheduled maintenance entity
        /// </summary>
        /// <param name="maintenance">Scheduled maintenance to merge</param>
        /// <returns>Scheduled maintenance</returns>
        public ScheduledMaintenance MergeScheduledMaintenance(ScheduledMaintenance maintenance)
        {
            return _scheduledMaintenanceRepository.Merge(maintenance);
        }

        /// <summary>
        /// Returns all upcoming scheduled maintenances
        /// </summary>
        /// <returns>List of all upcoming scheduled maintenances</returns>
        public IQueryable<ScheduledMaintenance> GetAllUpcomingMaintenances()
        {
            return _scheduledMaintenanceRepository.FilterBy(x => x.Service != null && x.Offline >= DateTime.Now);
        }

        /// <summary>
        /// Returns all upcoming scheduled maintenances within a specified number of days
        /// </summary>
        /// <param name="days">Number of days in future to check for scheduled maintenances</param>
        /// <returns>List of all upcoming scheduled maintenances</returns>
        public IQueryable<ScheduledMaintenance> GetAllUpcomingMaintenances(int days)
        {
            return _scheduledMaintenanceRepository.FilterBy(x => x.Service != null && x.Offline >= DateTime.Now && x.Offline <= DateTime.Now.AddDays(days));
        }

        /// <summary>
        /// Returns a scheduled maintenance by id
        /// </summary>
        /// <param name="id">Id of scheduled maintenance</param>
        /// <returns>Scheduled maintenance</returns>
        public ScheduledMaintenance GetScheduledMaintenanceById(int id)
        {
            return _scheduledMaintenanceRepository.FindBy(x => x.Id == id);
        }
    }
}
