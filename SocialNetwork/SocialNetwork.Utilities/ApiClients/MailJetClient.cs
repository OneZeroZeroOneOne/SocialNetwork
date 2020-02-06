using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Utilities.Abstractions;
using System.Threading.Tasks;

namespace SocialNetwork.Utilities.ApiClients
{
    public class MailJetClient : IMailClient
    {
        private readonly IConfiguration _config;

        private string _apiSecret;
        private string _apiKey;

        public MailJetClient(IConfiguration config)
        {
            _config = config;

            LoadSecrets();
        }

        private void LoadSecrets()
        {
            _apiSecret = _config.GetSection("MailJet").GetSection("ApiSecret").Value;
            _apiKey = _config.GetSection("MailJet").GetSection("ApiKey").Value;

            if (_apiKey == null || _apiSecret == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CantLoadSecrets, "Cant load secrets for Mail Jet Client");
        }

        public async Task<bool> SendConfirmationMail(string email, string confirmUrl)
        {
            if (_apiKey == null || _apiSecret == null)
                LoadSecrets();

            MailjetClient client = new MailjetClient(_apiKey, _apiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.Messages, new JArray
                {
                    new JObject
                    {
                        {
                            "From",
                            new JObject
                            {
                                {"Email", "zebestforevka@gmail.com"},
                                {"Name", "SocialNetwork"}
                            }
                        }, {
                            "To",
                            new JArray
                            {
                                new JObject
                                {
                                    {
                                        "Email", email
                                    }
                                }
                            }
                        }, {
                            "Subject",
                            "Confirm your email"
                        }, {
                            "TextPart",
                            "Confirm your email"
                        }, {
                            "HTMLPart",
                            $"<h3>Hello, thank for registering in SocialNetwork!</h3></br><a href='{confirmUrl}'>Confirm</a> your email for using our service!"
                        }
                    }
                });

            MailjetResponse response = await client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
