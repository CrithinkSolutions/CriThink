using System;

namespace CriThink.Server.Core.Validators
{
    internal abstract class BaseUriValidator
    {
        protected BaseUriValidator NextValidator;

        public void SetNext(BaseUriValidator validator)
        {
            NextValidator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public abstract string Validate(string domainToValidate);
    }
}