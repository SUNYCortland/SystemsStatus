using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class UsersEditViewModelValidator : AbstractValidator<UsersEditViewModel>
    {
        private readonly IServiceService _serviceService;
        private readonly IUserService _userService;

        public UsersEditViewModelValidator(IServiceService serviceService, IUserService userService)
        {
            _serviceService = serviceService;
            _userService = userService;

            RuleFor(x => x.ServiceIds).Must(BeValidService)
                .WithMessage("Invalid service selected. Please refresh the page and try again.");
        }

        public bool BeValidService(IList<int> ServiceIds)
        {
            foreach (var serviceId in ServiceIds)
            {
                var service = _serviceService.GetServiceById(serviceId);

                if (service == null)
                    return false;
            }

            return true;
        }
    }
}