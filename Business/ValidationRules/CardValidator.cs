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
            RuleFor(c => c.Cvc).Length(3);
            RuleFor(c => c.ExpireMonth).MaximumLength(2);
            RuleFor(c => c.ExpireYear).Length(4);
            RuleFor(c => c.CardNumber).MaximumLength(16);

        }
    }
}
