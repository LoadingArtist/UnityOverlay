using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

using UnityEngine;

public class Twitch_WebSocket : MonoBehaviour
{

    public delegate void ReceiveAction(string message);
    public event ReceiveAction OnReceived;
    private List<ArraySegment<byte>> sendBytesQueue = new List<ArraySegment<byte>>();
    private List<ArraySegment<byte>> sendTextQueue = new List<ArraySegment<byte>>();

    private ClientWebSocket webSocket = null;
    private CancellationToken m_CancellationToken;
    private CancellationTokenSource m_TokenSource;

    [SerializeField]
    private string channel_id = "CHANNEL_ID";
    [SerializeField]
    private string token = "TOKEN";

    void Start()
    {
        Task connect = Connect("wss://pubsub-edge.twitch.tv");

        // Keep sending messages at every 20s
        InvokeRepeating("SendPingMessage", 0.0f, 20f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SendPlainText("{  \"type\": \"LISTEN\",  \"nonce\": \"somerandomstringhere\",  \"data\": {    \"topics\": [\"channel-points-channel-v1.28156958\"],    \"auth_token\": \"" + token + "\"  }}");
        }
    }

    void OnDestroy()
    {
        if (webSocket != null)
            webSocket.Dispose();

        Debug.Log("WebSocket closed.");
    }

    async void SendWebSocketBytes()
    {
        if (webSocket.State == WebSocketState.Open)
        {
            await SendBytes(new byte[] { 10, 20, 30 });
        }
    }

    async void SendPingMessage()
    {
        if (webSocket.State == WebSocketState.Open)
        {
            // Sending plain text
            await SendPlainText("{\"type\": \"PING\"}");
        }
    }

    public Task SendBytes(byte[] bytes)
    {
        return SendMessage(sendBytesQueue, WebSocketMessageType.Binary, new ArraySegment<byte>(bytes));
    }


    public Task SendPlainText(string message)
    {
        var encoded = Encoding.UTF8.GetBytes(message);
        return SendMessage(sendTextQueue, WebSocketMessageType.Text, new ArraySegment<byte>(encoded, 0, encoded.Length));
    }

    public async Task Connect(string uri)
    {
        try
        {
            m_TokenSource = new CancellationTokenSource();
            m_CancellationToken = m_TokenSource.Token;
            webSocket = new ClientWebSocket();
            webSocket.Options.KeepAliveInterval = TimeSpan.Zero;
            await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);

            Debug.Log(webSocket.State);

            await Receive();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    bool isSending;
    private readonly object Lock = new object();

    private async Task SendMessage(List<ArraySegment<byte>> queue, WebSocketMessageType messageType, ArraySegment<byte> buffer)
    {
        await Task.Yield();

        if (buffer.Count == 0)
        {
            return;
        }

        bool sending;

        lock (Lock)
        {
            sending = isSending;

            if (!isSending)
            {
                isSending = true;
            }
        }

        if (!sending)
        {
            if (!Monitor.TryEnter(webSocket, 1000))
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, string.Empty, m_CancellationToken);
                return;
            }

            try
            {
                var t = webSocket.SendAsync(buffer, messageType, true, m_CancellationToken);
                t.Wait(m_CancellationToken);
            }
            finally
            {
                Monitor.Exit(webSocket);
            }

            lock (Lock)
            {
                isSending = false;
            }

            await HandleQueue(queue, messageType);
        }
        else
        {
            // Add the message to the queue.
            lock (Lock)
            {
                queue.Add(buffer);
            }
        }
    }

    private async Task HandleQueue(List<ArraySegment<byte>> queue, WebSocketMessageType messageType)
    {
        var buffer = new ArraySegment<byte>();
        lock (Lock)
        {
            // Check for an item in the queue.
            if (queue.Count > 0)
            {
                buffer = queue[0];
                queue.RemoveAt(0);
            }
        }

        // Send that message.
        if (buffer.Count > 0)
        {
            await SendMessage(queue, messageType, buffer);
        }
    }

    private async Task Receive()
    {
        ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[8192]);

        while (webSocket.State == WebSocketState.Open)
        {

            WebSocketReceiveResult result = null;

            using (var ms = new MemoryStream())
            {
                do
                {
                    result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {

                        string message = reader.ReadToEnd();
                        Debug.Log(message);
                        if (OnReceived != null) OnReceived(message);

                    }
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    string message = "";
                    if (OnReceived != null) OnReceived(message);
                }
            }
        }
    }
}
