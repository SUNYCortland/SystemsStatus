using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using Castle.MicroKernel;

namespace SystemsStatus.Common.Validation
{
    public class WindsorFluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IKernel _kernel;

        public WindsorFluentValidatorFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _kernel.HasComponent(validatorType)
                   ? _kernel.Resolve(validatorType) as IValidator
                   : null;
        }
    }
}
