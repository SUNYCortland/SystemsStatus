using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class ServicesEditViewModelValidator : AbstractValidator<ServicesEditViewModel>
    {
        private readonly IUserService _userService;

        public ServicesEditViewModelValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.OwnerIds).NotEmpty()
                .WithMessage("You must select at least one owner for this service.")
                .Must(ContainValidUserIds)
                .WithMessage("Invalid user selected. Please refresh the page and try again.");
        }

        public bool ContainValidUserIds(IList<int> userIds)
        {
            foreach (int userId in userIds)
            {
                if (_userService.GetUserById(userId) == null)
                    return false;
            }

            return true;
        }
    }
}