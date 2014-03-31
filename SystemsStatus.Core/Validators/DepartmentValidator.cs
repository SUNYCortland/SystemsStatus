using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using SystemsStatus.Core.Data.Entities;

namespace SystemsStatus.Core.Validators
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("You must specify a name for this department.");

            RuleFor(x => x.Code).NotEmpty()
                .WithMessage("You must specify a department code.")
                .Length(1, 4)
                .WithMessage("Department code must be 4 characters or less.");
        }
    }
}
