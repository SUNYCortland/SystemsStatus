using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentValidation;

namespace SystemsStatus.Core.Validators
{
    public class OfflineMaintenanceServiceStatusValidator : AbstractValidator<OfflineMaintenanceServiceStatus>
    {
        public OfflineMaintenanceServiceStatusValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("You must specify a name for this service status.");

            RuleFor(x => x.ScheduledMaintenance).NotEmpty()
                .WithMessage("You must specify a schedule maintenance for this service status.");
        }
    }
}
