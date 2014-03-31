using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentValidation;

namespace SystemsStatus.Core.Validators
{
    public class OnlineDegradedServiceStatusValidator : AbstractValidator<OnlineDegradedServiceStatus>
    {
        public OnlineDegradedServiceStatusValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("You must specify a name for this service status.");

            RuleFor(x => x.Cause).NotEmpty()
                .WithMessage("You must specify a cause for this service degradation.");

            RuleFor(x => x.DegradedSince).NotEmpty()
                .WithMessage("You must specify a date/time since this service has been degraded.");
        }
    }
}
