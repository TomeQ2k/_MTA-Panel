using FluentValidation;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;

namespace MTA.Core.Application.Validators
{
    public static class EmailAdressValidator
    {
        public static IRuleBuilderOptions<T, TElement> IsEmailAdress<T, TElement>(
            this IRuleBuilderOptions<T, TElement> ruleBuilder)
            => ruleBuilder.Must(x => (x as string).IsEmailAddress())
                .WithMessage(ValidatorMessages.EmailAddressValidatorMessage);
    }
}