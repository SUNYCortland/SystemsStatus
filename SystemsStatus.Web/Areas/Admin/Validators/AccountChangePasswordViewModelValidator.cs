using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Core.Services;
using SystemsStatus.Common.Authentication;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class AccountChangePasswordViewModelValidator : AbstractValidator<AccountChangePasswordViewModel>
    {
        private readonly IUserService _userService;

        public AccountChangePasswordViewModelValidator(IUserService userService)
        {
            _userService = userService;


            RuleFor(x => x.CurrentPassword).NotEmpty()
                .WithMessage("You must specify your current password.")
                .Must(BeCorrectPassword)
                .WithMessage("The current password specified is incorrect.");

            RuleFor(x => x.NewPassword).NotEmpty()
                .WithMessage("You must specify a new password.")
                .Must(MatchRepeatedPassword)
                .WithMessage("The specified passwords do not match.");

            RuleFor(x => x.NewPasswordRepeat).NotEmpty()
                .WithMessage("You must specify a repeated password.");


        }

        public bool BeCorrectPassword(string Password)
        {
            var user = _userService.GetUserByUsername(HttpContext.Current.User.Identity.Name);

            if(!String.IsNullOrWhiteSpace(Password))
                if (user != null)
                    if (user.HashedPassword == AuthPasswordHelper.HashEncode(Password, user.PasswordSalt, 100))
                        return true;

            return false;
        }

        public bool MatchRepeatedPassword(AccountChangePasswordViewModel instance, string Password)
        {
            return Password == instance.NewPasswordRepeat;
        }
    }
}