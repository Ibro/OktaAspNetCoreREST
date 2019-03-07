using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Client
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
    
    public class TokenService : ITokenService
    {
        private OktaToken _token = new OktaToken();
        private readonly IOptions<OktaConfig> _oktaSettings;

        public TokenService(IOptions<OktaConfig> oktaSettings) => _oktaSettings = oktaSettings;

        public async Task<string> GetToken()
        {
	        if (_token.IsValidAndNotExpiring)
	        {
		        return _token.AccessToken;
	        }

	        _token = await GetNewAccessToken();

            return _token.AccessToken;
        }

        private async Task<OktaToken> GetNewAccessToken()
        {
	        var client = new HttpClient();
	        var clientId = _oktaSettings.Value.ClientId;
	        var clientSecret = _oktaSettings.Value.ClientSecret;
	        var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");

	        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

	        var postMessage = new Dictionary<string, string>
	        {
		        {"grant_type", "client_credentials"}, 
		        {"scope", "access_token"}
	        };

	        var request = new HttpRequestMessage(HttpMethod.Post, _oktaSettings.Value.TokenUrl)
	        {
		        Content = new FormUrlEncodedContent(postMessage)
	        };

	        var response = await client.SendAsync(request);
	        if (response.IsSuccessStatusCode)
	        {
		        var json = await response.Content.ReadAsStringAsync();
                var newToken = JsonConvert.DeserializeObject<OktaToken>(json);
                newToken.ExpiresAt = DateTime.UtcNow.AddSeconds(_token.ExpiresIn);

                return newToken;
	        }

	        throw new ApplicationException("Unable to retrieve access token from Okta");
        }
    }
}