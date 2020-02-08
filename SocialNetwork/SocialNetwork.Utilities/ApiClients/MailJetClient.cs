using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using SocialNetwork.Utilities.Abstractions;
using SocialNetwork.Utilities.Exceptions;
using System.Threading.Tasks;

namespace SocialNetwork.Utilities.ApiClients
{
    public class MailJetClient : IMailClient
    {
        private readonly IConfigSettingService _config;

        private string _apiSecret;
        private string _apiKey;

        public MailJetClient(IConfigSettingService config)
        {
            _config = config;

            LoadSecrets();
        }

        private void LoadSecrets()
        {
            _apiSecret = _config.GetSetting<string>("MailJet_ApiSecret", null);
            _apiKey = _config.GetSetting<string>("MailJet_ApiKey", null);

            if (_apiKey == null || _apiSecret == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CantLoadSecrets, "Cant load secrets for Mail Jet Client");
        }

        private async Task LoadSecretsAsync()
        {
            _apiSecret = await _config.GetSettingAsync<string>("MailJet_ApiSecret", null);
            _apiKey = await _config.GetSettingAsync<string>("MailJet_ApiKey", null);

            if (_apiKey == null || _apiSecret == null)
                throw ExceptionFactory.SoftException(ExceptionEnum.CantLoadSecrets, "Cant load secrets for Mail Jet Client");
        }

        public async Task<bool> SendConfirmationMail(string email, string confirmUrl)
        {
            if (_apiKey == null || _apiSecret == null)
                await LoadSecretsAsync();

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
