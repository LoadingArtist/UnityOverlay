using System;
using Assets.Helpers;
using Assets.Models;
using Assets.Services;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Testing
{
    public class TwitchPubSub : MonoBehaviour
    {
        private WebSocket ws;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void HeartBeat()
        {
            if (ws.ReadyState == WebSocketState.Open)
            {
                Debug.Log("HEARTBEAT");
                ws.Send("{ \"type\":\"PING\"}");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            try
            {
                const string user = "";
                const string twitchOAuthToken = "";
                const string clientId = "";

                ws = new WebSocket("wss://pubsub-edge.twitch.tv");
                InvokeRepeating("HeartBeat", 0.0f, 20f);

                TwitchApiService apiService = new TwitchApiService(twitchOAuthToken, clientId);

                ws.OnOpen += async (sender, args) =>
                {
                    Debug.Log("SOCKET OPENED");

                    var userRes = await apiService.GetUser(user);
                    var listenerMessage = TwitchMessageHelper.CreateListenerMessage(twitchOAuthToken, $"channel-points-channel-v1.{userRes.id}");
                    ws.Send(listenerMessage);
                };

                ws.OnError += (sender, args) =>
                {
                    Debug.Log("ERROR");
                    Debug.Log(args.Message);
                };

                ws.OnMessage += (sender, args) =>
                {
                    var res = JsonUtility.FromJson<ApiResponse<TwitchMessageHeader>>(args.Data);
                    switch (res.type)
                    {
                        case "PONG":
                            break;
                        case "RESPONSE":
                            break;
                        case "MESSAGE":
                            var innerRes = JsonUtility.FromJson<ApiResponse<string>>(res.data.message);
                            if (innerRes.type == "reward-redeemed")
                            {
                                var reward = JsonUtility.FromJson<TwitchRewardRedemptionHeader>(innerRes.data);
                            }
                            break;
                        default:
                            break;
                    }

                    Debug.Log($"MESSAGE TYPE:{res.type}");
                };

                ws.OnClose += (sender, args) =>
                {
                    Debug.Log($"SOCKET CLOSED REASON:{args.Reason} CODE:{args.Code}");
                    
                    //Reconnect if socket closed unexpectedly
                    if (args.Code == 1006)
                    {
                        Debug.Log("RECONNECTING");
                        ws.Connect();
                    }
                };

                ws.ConnectAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void OnDisable()
        {
            ws.CloseAsync();
        }



        [ContextMenu("Send Donation")]
        private void SendDonation()
        {

        }
    }

}
