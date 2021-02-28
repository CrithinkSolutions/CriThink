namespace CriThink.Server.Core.Validators
{
    internal class UriStringValidator : BaseUriValidator
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
