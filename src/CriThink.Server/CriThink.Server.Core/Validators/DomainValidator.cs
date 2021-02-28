namespace CriThink.Server.Core.Validators
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
                _schemaValidator.Validate(domain);
        }
    }
}