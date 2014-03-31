using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentValidation;

namespace SystemsStatus.Core.Validators
{
    public class WidgetValidator : AbstractValidator<Widget>
    {
        public WidgetValidator()
        {
            RuleFor(x => x.Height)
                .NotEmpty()
                .WithMessage("You must specify the height of this widget.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("You must specify a name for this widget.")
                .Length(1, 200)
                .WithMessage("Widget name must be less than or equal to 200 characters.");
        }
    }
}
