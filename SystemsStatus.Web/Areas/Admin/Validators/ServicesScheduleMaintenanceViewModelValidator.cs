using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class ServicesScheduleMaintenanceViewModelValidator : AbstractValidator<ServicesScheduleMaintenanceViewModel>
    {
        private readonly IServiceService _serviceService;

        public ServicesScheduleMaintenanceViewModelValidator(IServiceService serviceService)
        {
            _serviceService = serviceService;

            RuleFor(x => x.ServiceIds).NotEmpty()
                .WithMessage("You must select at least one service to schedule maintenance for.");

            RuleFor(x => x.ScheduledMaintenance.Offline).Must(NotHaveConflictingScheduledMaintenance)
                .WithMessage("This scheduled maintenance conflicts with one or more selected services.");
        }

        public bool NotHaveConflictingScheduledMaintenance(ServicesScheduleMaintenanceViewModel instance, DateTime Offline)
        {
            foreach (int serviceId in instance.ServiceIds)
            {
                var service = _serviceService.GetServiceById(serviceId);

                foreach (var scheduledMaintenance in service.Maintenances)
                {
                    // Check for Offline between
                    if (Offline <= scheduledMaintenance.ExpectedOnline
                        && Offline >= scheduledMaintenance.Offline)
                        return false;

                    // Check for expected online between another maintenance
                    if (Offline <= scheduledMaintenance.Offline
                        && instance.ScheduledMaintenance.ExpectedOnline >= scheduledMaintenance.Offline)
                        return false;
                }
            }

            return true;
        }
    }
}