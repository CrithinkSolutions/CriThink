using System;

namespace CriThink.Server.Core.Validators
{
    internal class SchemaValidator : BaseUriValidator
    {
        public override string Validate(string domainToValidate)
        {
            if (string.IsNullOrWhiteSpace(domainToValidate))
                return string.Empty;

            var uriBuilder = new UriBuilder(domainToValidate);
            var validatedUrl = uriBuilder.Host;

            return NextValidator is null ?
                validatedUrl :
                NextValidator.Validate(validatedUrl);
        }
    }
}