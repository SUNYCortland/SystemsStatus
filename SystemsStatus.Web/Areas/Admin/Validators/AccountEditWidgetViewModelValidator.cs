using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class AccountEditWidgetViewModelValidator : AbstractValidator<AccountEditWidgetViewModel>
    {
        public AccountEditWidgetViewModelValidator()
        {
            RuleFor(x => x.ServiceIds)
                .NotEmpty()
                .When(x => !x.DepartmentId.HasValue)
                .WithMessage("You must select at least one service for this widget.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .When(x => x.ServiceIds.Count() == 0)
                .WithMessage("You must select a department for this widget.");
        }
    }
}