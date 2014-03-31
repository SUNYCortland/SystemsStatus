using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SystemsStatus.Web.Areas.Admin.ViewModels;
using SystemsStatus.Core.Services;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class DepartmentsCreateViewModelValidator : AbstractValidator<DepartmentsCreateViewModel>
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsCreateViewModelValidator(IDepartmentService departmentService)
        {
            _departmentService = departmentService;

            RuleFor(x => x.Department.Name).Must(BeUniqueName)
                .WithMessage("A department with this name already exists. Please choose another.");

            RuleFor(x => x.Department.Code).Must(BeUniqueCode)
                .WithMessage("A department with this department code already exists. Please choose another.");
        }

        public bool BeUniqueCode(string Code)
        {
            if (!String.IsNullOrWhiteSpace(Code))
                return _departmentService.GetAllDepartments().Where(x => x.Code.ToLower() == Code.ToLower()).Count() == 0;
            else
                return false;
        }

        public bool BeUniqueName(string Name)
        {
            if (!String.IsNullOrWhiteSpace(Name))
                return _departmentService.GetAllDepartments().Where(x => x.Name.ToLower() == Name.ToLower()).Count() == 0;
            else
                return false;
        }
    }
}