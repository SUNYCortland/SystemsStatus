using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentValidation;

namespace SystemsStatus.Core.Validators
{
    public class OnlineServiceStatusValidator : AbstractValidator<OnlineServiceStatus>
    {
        public OnlineServiceStatusValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("You must specify a name for this service status.");

            RuleFor(x => x.OnlineSince).NotEmpty()
                .WithMessage("You must specify a time since this service has been online.");
        }
    }
}
