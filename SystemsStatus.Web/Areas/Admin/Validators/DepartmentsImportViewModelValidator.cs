using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using SystemsStatus.Web.Areas.Admin.ViewModels;

namespace SystemsStatus.Web.Areas.Admin.Validators
{
    public class DepartmentsImportViewModelValidator : AbstractValidator<DepartmentsImportViewModel>
    {
        public DepartmentsImportViewModelValidator()
        {
            RuleFor(x => x.DepartmentsFile).NotEmpty()
                .When(x => String.IsNullOrEmpty(x.DepartmentsXml))
                .WithMessage("You must either copy/paste XML or upload an XML File.");

            RuleFor(x => x.DepartmentsXml).NotEmpty()
                .When(x => x.DepartmentsFile == null)
                .WithMessage("You must either copy/paste XML or upload an XML File.");
        }
    }
}