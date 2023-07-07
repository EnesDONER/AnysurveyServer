using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class AdFilterValidator : AbstractValidator<AdFilter>
    {
        public AdFilterValidator()
        {
            RuleFor(af => af.MinAge)
             .LessThanOrEqualTo(af => af.MaxAge)
             .WithMessage("'MinAge' must be less than or equal to 'MaxAge'");

        }
    }
}

