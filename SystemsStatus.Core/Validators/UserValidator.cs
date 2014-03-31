using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Core.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly IUserRoleService _userRoleService;

        public UserValidator(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;

            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("You must specify a first name.");

            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("You must specify a last name.");

            RuleFor(x => x.Username).NotEmpty()
                .WithMessage("You must specify a username.");

            RuleFor(x => x.Role).NotEmpty()
                .WithMessage("You must select a role for this user.");
        }

        public bool BeValidRole(UserRole Role)
        {
            return _userRoleService.GetUserRoleById(Role.Id.Value) != null;
        }
    }
}
