using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            RuleFor(c => c.CVC).Length(3);
            RuleFor(c => c.Month).Length(2);
            RuleFor(c => c.Year).Length(4);

        }
    }
}
