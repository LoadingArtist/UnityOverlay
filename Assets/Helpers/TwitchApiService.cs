using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Assets.Models;
using UnityEngine;

namespace Assets.Helpers
{
    public class TwitchApiService
    {
        private HttpClient client;
        private const string baseUri = "https://api.twitch.tv/helix";

        public TwitchApiService(string TwitchOAuthToken, string ClientId)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {TwitchOAuthToken}");
            client.DefaultRequestHeaders.Add("client-id", ClientId);
        }

        public async Task<TwitchUser> GetUser(string username)
        {
            var response = await client.GetAsync($"{baseUri}/users?login={username}");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var res = JsonUtility.FromJson<ApiResponse<List<TwitchUser>>>(jsonResponse);
            return res.data.SingleOrDefault();
        }
        
        ~TwitchApiService()
        {
            client.Dispose();
        }
    }
}
