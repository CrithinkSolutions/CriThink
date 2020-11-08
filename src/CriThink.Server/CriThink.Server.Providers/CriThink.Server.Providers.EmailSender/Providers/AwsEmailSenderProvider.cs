using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CriThink.Server.Providers.EmailSender.Providers
{
    public class AwsEmailSenderProvider : IEmailSenderProvider
    {
        public Task Send(string fromAddress, IEnumerable<string> recipients, string subject, string htmlBody)
        {
            using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUCentral1);
            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string>(recipients)
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlBody
                        }
                    }
                }
            };

            return client.SendEmailAsync(sendRequest);
        }
    }
}
