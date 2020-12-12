using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using UnityEngine;
using Random = System.Random;

namespace Assets.Services
{
    public static class TwitchMessageHelper
    {
        public static string CreateListenerMessage(string twitchOAuthToken, List<string> topics)
        {
            TwitchListenMessage listenerMessage = new TwitchListenMessage();
            listenerMessage.nonce = nonce(15);

            var lData = new TwitchListenMessageData();
            lData.auth_token = twitchOAuthToken;
            lData.topics = new List<string>();
            foreach (var topic in topics)
            {
                lData.topics.Add(topic);
            }

            listenerMessage.data = lData;

            return JsonUtility.ToJson(listenerMessage);
        }

        public static string CreateListenerMessage(string twitchOAuthToken, string topic)
        {
            return CreateListenerMessage(twitchOAuthToken, new List<string>{topic});
        }

        private static string nonce(int length)
        {
            Random random = new Random();
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (var i = 0; i < length; i++)
            {
                text += possible[random.Next(0, possible.Length - 1)];
            }
            return text;
        }
    }
}
