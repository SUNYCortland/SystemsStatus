using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class UsersCreateViewModelValidator : AbstractValidator<UsersCreateViewModel>
    {
        private readonly IUserService _userService;

        public UsersCreateViewModelValidator(IUserService userService)
        {
            _userService = userService;
            
            RuleFor(x => x.User.Username).Must(BeUniqueUsername)
                .WithMessage("A user with this username already exists. Please choose another.");

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("You must specify a password.");

            RuleFor(x => x.PasswordRepeat).NotEmpty()
                .WithMessage("You must specify a repeated password.");

            RuleFor(x => x.Password).Must(MatchRepeatedPassword)
                .WithMessage("The specified passwords do not match.");
        }

        public bool BeUniqueUsername(string Username)
        {
            if (!String.IsNullOrWhiteSpace(Username))
                return _userService.GetAllUsers().Where(x => x.Username.ToLower() == Username.ToLower()).Count() == 0;
            else
                return false;
        }

        public bool MatchRepeatedPassword(UsersCreateViewModel instance, string Password)
        {
            return Password == instance.PasswordRepeat;
        }
    }
}