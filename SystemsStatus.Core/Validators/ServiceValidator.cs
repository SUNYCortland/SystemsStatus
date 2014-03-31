using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Core.Validators
{
    public class ServiceValidator : AbstractValidator<Service>
    {
        public ServiceValidator()
        {
            RuleFor(service => service.Name)
                .NotEmpty()
                .WithMessage("You must specify a name for this service.")
                .Length(1, 200)
                .WithMessage("Service name must be less than or equal to 200 characters.");

            RuleFor(service => service.Description)
                .NotEmpty()
                .WithMessage("You must specify a description for this service.")
                .Length(1, 2000)
                .WithMessage("Service description must be less than or equal to 2000 characters.");

            RuleFor(x => x.Tags).Must(HaveUniqueTags)
                .When(x => x.Tags.Count > 0)
                .WithMessage("All tags must be unique.");
        }

        public bool HaveUniqueTags(ICollection<string> Tags)
        {
            return Tags.GroupBy(x => x.ToUpper()).Where(y => y.Count() == 1).Any();
        }
    }
}
