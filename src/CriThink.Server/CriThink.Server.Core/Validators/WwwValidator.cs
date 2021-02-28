namespace CriThink.Server.Core.Validators
{
    internal class WwwValidator : BaseUriValidator
    {
        public override string Validate(string domainToValidate)
        {
            if (string.IsNullOrWhiteSpace(domainToValidate))
                return string.Empty;

            var validatedUrl = domainToValidate.Replace("www.", "");

            return NextValidator is null ?
                validatedUrl :
                NextValidator.Validate(validatedUrl);
        }
    }
}