using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class SurveyValidator : AbstractValidator<Survey>
    {
        public SurveyValidator()
        {
            //RuleFor(c => c.ModelYear).NotEmpty();
            //RuleFor(c => c.DailyPrice).NotEmpty();
            //RuleFor(c => c.DailyPrice).GreaterThan(0);
            //RuleFor(c => c.DailyPrice).GreaterThanOrEqualTo(200).When(c => c.ColorId == 1);
            //RuleFor(c => c.Description).Must(StartWithA).WithMessage("ürünler A harfi ile başlamalı");
        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
