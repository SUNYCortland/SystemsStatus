using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using System.ComponentModel.DataAnnotations;

namespace SystemsStatus.Core.Data.Entities
{
    public class Service : Entity<int?>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<Service> Dependents { get; set; }

        public virtual Service Parent { get; set; }

        public virtual ServiceStatus CurrentStatus { get; set; }

        public virtual ICollection<ServiceStatus> Statuses { get; set; }

        public virtual IEnumerable<ScheduledMaintenance> Maintenances { get; set; }

        public virtual ICollection<User> Owners { get; set; }

        public virtual IEnumerable<Service> Children { get; set; }

        public virtual Department Department { get; set; }

        public virtual string SLA { get; set; }

        public virtual string Cost { get; set; }

        [Display(Name = "Dashboard")]
        public virtual bool Public { get; set; }

        public virtual string Url { get; set; }

        public virtual ICollection<string> Tags { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual int SortOrder { get; set; }

        public virtual double PercentOnline
        {
            get
            {
                // Calculate total online hours
                var totalOnlineHours = CalculateTotalOnlineHours();

                // Calculate total offline hours
                var totalOfflineHours = CalculateTotalOfflineUnplannedHours();

                // Calculate total maintenance hours
                var totalMaintenanceHours = CalculateTotalOfflineMaintenanceHours();

                // Calculate total degraded hours
                var totalDegradedHours = CalculateTotalOnlineDegradedHours();

                // Get percentage online
                return ((totalOnlineHours - totalOfflineHours - totalMaintenanceHours - totalDegradedHours) / (totalOnlineHours));
            }
        }

        public virtual double PercentOffline
        {
            get
            {
                // Calculate total online hours
                var totalOnlineHours = CalculateTotalOnlineHours();

                // Calculate total offline hours
                var totalOfflineHours = CalculateTotalOfflineUnplannedHours();

                return (totalOfflineHours / totalOnlineHours);
            }
        }

        public virtual double PercentMaintenance
        {
            get
            {
                // Calculate total online hours
                var totalOnlineHours = CalculateTotalOnlineHours();

                // Calculate total maintenance hours
                var totalMaintenanceHours = CalculateTotalOfflineMaintenanceHours();

                return (totalMaintenanceHours / totalOnlineHours);
            }
        }

        public virtual double PercentDegraded
        {
            get
            {
                // Calculate total online hours
                var totalOnlineHours = CalculateTotalOnlineHours();

                // Calculate total degraded hours
                var totalDegraded = CalculateTotalOnlineDegradedHours();

                return (totalDegraded / totalOnlineHours);
            }
        }

        private double CalculateTotalOnlineHours()
        {
            // Calculate total online hours
            var onlineServiceStatuses = this.Statuses.Where(x => x.GetType() == typeof(OnlineServiceStatus));
            if (onlineServiceStatuses == null 
                || onlineServiceStatuses.Count() == 0)
                return 0.0;

            var minOnlineServiceStatus = onlineServiceStatuses
                .Select(x => x as OnlineServiceStatus)
                .Min(x => x.OnlineSince);

            return (DateTime.Now - minOnlineServiceStatus).TotalHours;
        }

        private double CalculateTotalOfflineUnplannedHours()
        {
            // Calculate total offline hours
            var offlineUnplannedServiceStatuses = this.Statuses.Where(x => x.GetType() == typeof(OfflineUnplannedServiceStatus));
            var totalOfflineHours = 0.0;
            if (offlineUnplannedServiceStatuses != null
                && offlineUnplannedServiceStatuses.Count() > 0)
            {
                totalOfflineHours = offlineUnplannedServiceStatuses
                                        .Select(x => x as OfflineUnplannedServiceStatus)
                                        .Sum(x => ((x.ActualOnline.HasValue ? x.ActualOnline.Value : DateTime.Now) - x.OfflineSince).TotalHours);
            }

            return totalOfflineHours;
        }

        private double CalculateTotalOfflineMaintenanceHours()
        {
            // Calculate total maintenance hours
            var offlineMaintenanceServiceStatuses = this.Statuses.Where(x => x.GetType() == typeof(OfflineMaintenanceServiceStatus));
            var totalMaintenanceHours = 0.0;
            if (offlineMaintenanceServiceStatuses != null
                && offlineMaintenanceServiceStatuses.Count() > 0)
            {
                totalMaintenanceHours = offlineMaintenanceServiceStatuses
                                            .Select(x => x as OfflineMaintenanceServiceStatus)
                                            .Sum(x => ((x.ScheduledMaintenance.Online.HasValue ? x.ScheduledMaintenance.Online.Value : DateTime.Now) - x.ScheduledMaintenance.Offline).TotalHours);
            }

            return totalMaintenanceHours;
        }

        private double CalculateTotalOnlineDegradedHours()
        {
            // Calculate total degraded hours
            var onlineDegradedServiceStatuses = this.Statuses.Where(x => x.GetType() == typeof(OnlineDegradedServiceStatus));
            var totalDegradedHours = 0.0;
            if (onlineDegradedServiceStatuses != null
                && onlineDegradedServiceStatuses.Count() > 0)
            {
                totalDegradedHours = onlineDegradedServiceStatuses
                                        .Select(x => x as OnlineDegradedServiceStatus)
                                        .Sum(x => ((x.ActualOnline.HasValue ? x.ActualOnline.Value : DateTime.Now) - x.DegradedSince).TotalHours);
            }

            return totalDegradedHours;
        }

        public Service()
        {
            this.Dependents = new HashSet<Service>();
            this.Statuses = new HashSet<ServiceStatus>();
            this.Owners = new HashSet<User>();
            this.Children = new List<Service>();
            this.Tags = new List<string>();
            this.Maintenances = new List<ScheduledMaintenance>();
            this.Public = true;
        }
    }
}
