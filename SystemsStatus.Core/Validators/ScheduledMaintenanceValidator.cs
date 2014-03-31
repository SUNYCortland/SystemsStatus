using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentValidation;

namespace SystemsStatus.Core.Validators
{
    public class ScheduledMaintenanceValidator : AbstractValidator<ScheduledMaintenance>
    {
        public ScheduledMaintenanceValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("You must specify a name for this scheduled maintenance.");

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage("You must specify a description for this scheduled maintenance.");

            RuleFor(x => x.Offline).NotEmpty()
                .WithMessage("You must specify a date and time that this service will go offline.");

            RuleFor(x => x.Offline).Must(BeLessThanExpectedOnline)
                .WithMessage("The offline date/time must happen prior to your expected online date/time.");

            RuleFor(x => x.Offline).GreaterThan(DateTime.Now)
                .WithMessage("You must schedule this maintenance to occur sometime in the future.");

            RuleFor(x => x.ExpectedOnline)
                .GreaterThan(x => x.Offline)
                .When(x => x.ExpectedOnline.HasValue)
                .WithMessage("The expected online date/time must happen after the expected online date/time.");
        }

        public bool BeLessThanExpectedOnline(ScheduledMaintenance Entity, DateTime Offline)
        {
            if (Entity.ExpectedOnline.HasValue)
            {
                if (Offline > Entity.ExpectedOnline)
                    return false;
            }

            return true;
        }
    }
}
