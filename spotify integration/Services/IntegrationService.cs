using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using RestSharp;
using System.Text;
using System.Text.Json;

namespace WebApplication1.Services
{
    public class IntegrationService: IIntegrationService
    {
        private IConfiguration Configuration { get; set; }

        private RestClient RestClient { get; set; }

        public IntegrationService(IConfiguration configuration, RestClient restClient)
        {
            this.Configuration = configuration;
            this.RestClient= restClient;
        }

        public string AddNewAccountToIntegration(IntegrationType integrationType)
        {
            var clientId = this.Configuration.GetValue<string>("SpotifyClientId");
            var redirectionUrl = this.Configuration.GetValue<string>("SpotifyRedirectionUrl");
            var scopes = "ugc-image-upload user-read-playback-state app-remote-control playlist-read-private user-modify-playback-state user-follow-modify playlist-read-collaborative user-follow-read user-read-currently-playing user-read-playback-position user-library-modify playlist-modify-private playlist-modify-public user-read-email user-top-read user-read-recently-played user-read-private user-library-read";
            var request = new RestRequest(this.Configuration.GetValue<string>("spotifyAuthorizationUrl"))
                .AddQueryParameter("response_type", "code")
                .AddQueryParameter("client_id", clientId)
                .AddQueryParameter("scope", scopes)
                .AddQueryParameter("redirect_uri", redirectionUrl);

            return RestClient.BuildUri(request).ToString();
        }

        public void HandleCallBack(string code)
        {
            if(code == null)
            {
                throw new Exception("An Error Occured");
            }

            try
            {
                var clientId = this.Configuration.GetValue<string>("SpotifyClientId");
                var clientSecretKey = this.Configuration.GetValue<string>("SpotifySecretKey");
                var request = new RestRequest(this.Configuration.GetValue<string>("SpotifyAuthTokenUrl"));
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ':' + clientSecretKey)));
                request.AddParameter("code", code);
                request.AddParameter("redirect_uri", this.Configuration.GetValue<string>("SpotifyRedirectionUrl"));
                request.AddParameter("grant_type", "authorization_code");
                var response = RestClient.PostAsync(request).Result;
                var content = JsonSerializer.Deserialize<TokenResponse>(response.Content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var accessToken = content.Access_token;
                var refreshToken = content.Refresh_token;

                // store the access token and refresh token in db and update them when ever accessing
            }

            catch (Exception ex)
            {
                throw new Exception("error occured " + ex.Message);
            }
        }
    }
}
