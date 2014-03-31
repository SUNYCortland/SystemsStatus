using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class ServicesChangeStatusViewModelValidator : AbstractValidator<ServicesChangeStatusViewModel>
    {
        private readonly IServiceService _serviceService;

        public ServicesChangeStatusViewModelValidator(IServiceService serviceService)
        {
            _serviceService = serviceService;

            RuleFor(x => x.ServiceIds).NotEmpty()
                .WithMessage("You must select at least one service to add this status to.");

            RuleFor(x => x.OnlineServiceStatus).NotEmpty()
                .When(x => x.OfflineMaintenanceServiceStatus == null
                    && x.OfflineUnplannedServiceStatus == null
                    && x.OnlineDegradedServiceStatus == null)
                .WithMessage("You must specify a service status.");

            RuleFor(x => x.OfflineMaintenanceServiceStatus).NotEmpty()
                .When(x => x.OnlineServiceStatus == null
                    && x.OfflineUnplannedServiceStatus == null
                    && x.OnlineDegradedServiceStatus == null)
                .WithMessage("You must specify a service status.");

            RuleFor(x => x.OfflineUnplannedServiceStatus).NotEmpty()
                .When(x => x.OfflineMaintenanceServiceStatus == null
                    && x.OnlineServiceStatus == null
                    && x.OnlineDegradedServiceStatus == null)
                .WithMessage("You must specify a service status.");

            RuleFor(x => x.OnlineDegradedServiceStatus).NotEmpty()
                .When(x => x.OfflineMaintenanceServiceStatus == null
                    && x.OfflineUnplannedServiceStatus == null
                    && x.OnlineServiceStatus == null)
                .WithMessage("You must specify a service status.");

            RuleFor(x => x.StatusType).NotEmpty()
                .WithMessage("You must select a status type.");
        }

        public bool BeOwnerOfServices(IList<int> ServiceIds)
        {
            foreach (int serviceId in ServiceIds)
            {
                var service = _serviceService.GetServiceById(serviceId);

                // Make sure currently logged in user is an owner of all services selected
                if (!service.Owners.Any(x => x.Username == HttpContext.Current.User.Identity.Name))
                {
                    return false;
                }
            }

            return true;
        }
    }
}