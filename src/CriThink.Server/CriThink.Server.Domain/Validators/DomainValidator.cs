using System;

namespace CriThink.Server.Domain.Validators
{
    internal class DomainValidator
    {
        private readonly BaseUriValidator _schemaValidator;

        public DomainValidator()
        {
            _schemaValidator = new SchemaValidator();
            BaseUriValidator wwwValidator = new WwwValidator();
            BaseUriValidator uriStringValidator = new UriStringValidator();

            _schemaValidator.SetNext(wwwValidator);
            wwwValidator.SetNext(uriStringValidator);
        }

        public string ValidateDomain(string domain)
        {
            return string.IsNullOrWhiteSpace(domain) ?
                string.Empty :
                _schemaValidator.Validate(domain.Trim());
        }

        private abstract class BaseUriValidator
        {
            protected BaseUriValidator NextValidator;

            public void SetNext(BaseUriValidator validator)
            {
                NextValidator = validator ?? throw new ArgumentNullException(nameof(validator));
            }

            public abstract string Validate(string domainToValidate);
        }

        private class SchemaValidator : BaseUriValidator
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

        private class WwwValidator : BaseUriValidator
        {
            private const string Www = "www.";

            public override string Validate(string domainToValidate)
            {
                if (string.IsNullOrWhiteSpace(domainToValidate))
                    return string.Empty;

                var validatedUrl = domainToValidate.StartsWith(Www)
                    ? domainToValidate.Replace(Www, "")
                    : domainToValidate;

                return NextValidator is null ?
                    validatedUrl :
                    NextValidator.Validate(validatedUrl);
            }
        }

        private class UriStringValidator : BaseUriValidator
        {
            public override string Validate(string domainToValidate)
            {
                if (string.IsNullOrWhiteSpace(domainToValidate))
                    return string.Empty;

                if (domainToValidate.EndsWith("/"))
                    domainToValidate = domainToValidate.Substring(0, domainToValidate.Length - 2);

                var validatedString = domainToValidate.ToUpperInvariant();
                return NextValidator is null ?
                    validatedString :
                    NextValidator.Validate(validatedString);
            }
        }
    }
}