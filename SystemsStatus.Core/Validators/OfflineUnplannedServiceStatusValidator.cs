using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities.Statuses;
using FluentValidation;

namespace SystemsStatus.Core.Validators
{
    public class OfflineUnplannedServiceStatusValidator : AbstractValidator<OfflineUnplannedServiceStatus>
    {
        public OfflineUnplannedServiceStatusValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("You must specify a name for this service status.");

            RuleFor(x => x.OfflineCause).NotEmpty()
                .WithMessage("You must specify a cause for this service going offline.");

            RuleFor(x => x.OfflineSince).NotEmpty()
                .WithMessage("You must specify a time that this service has been offline since.");
        }
    }
}
