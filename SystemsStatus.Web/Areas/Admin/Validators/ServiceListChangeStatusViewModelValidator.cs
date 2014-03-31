using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SystemsStatus.Web.Areas.Admin.ViewModels;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class ServiceListChangeStatusViewModelValidator : AbstractValidator<ServiceListChangeStatusViewModel>
    {
        public ServiceListChangeStatusViewModelValidator()
        {
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
    }
}