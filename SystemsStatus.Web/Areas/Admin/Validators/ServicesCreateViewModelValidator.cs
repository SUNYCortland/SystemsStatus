using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using FluentValidation;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class ServicesCreateViewModelValidator : AbstractValidator<ServicesCreateViewModel>
    {
        private readonly IServiceService _serviceService;

        public ServicesCreateViewModelValidator(IServiceService serviceService)
        {
            _serviceService = serviceService;

            RuleFor(x => x.Service).NotEmpty()
                .WithMessage("You must specify a service to create.");

            RuleFor(x => x.Service.Name).Must(BeUniqueServiceName)
                .WithMessage("A service with this name already exists. Please choose another.");

            RuleFor(x => x.StatusType).NotEmpty()
                .WithMessage("You must specify a status type.");

            RuleFor(x => x.OnlineServiceStatus).NotEmpty()
                .WithMessage("You must specify a service status.");

            RuleFor(x => x.ParentServiceId).Must(BeValidService)
                .WithMessage("You have selected an invalid parent service. Please select another.");
        }

        public bool BeValidService(int? ParentServiceId)
        {
            if (!ParentServiceId.HasValue)
                return true;

            var service = _serviceService.GetServiceById(ParentServiceId.Value);

            if (service == null)
                return false;

            return true;
        }

        public bool BeUniqueServiceName(string ServiceName)
        {
            if (!String.IsNullOrWhiteSpace(ServiceName))
                return _serviceService.GetAllServices().Where(x => x.Name.ToLower() == ServiceName.ToLower()).Count() == 0;
            else
                return false;
        }
    }
}