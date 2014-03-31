using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SystemsStatus.Web.Areas.Admin.ViewModels;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty()
                .WithMessage("You must specify a username.");

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("You must specify a password.");
        }
    }
}