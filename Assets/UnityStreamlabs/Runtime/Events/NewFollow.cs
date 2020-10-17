using System;
using System.Collections.Generic;

namespace UnityStreamlabs
{
    // https://dev.streamlabs.com/docs/socket-api

    [Serializable]
    public class NewFollow
    {
        public const string Type = "follow";

        [Serializable]
        public class Message
        {
            public string created_at;
            public string id;
            public string name;
            public string _id;
        }
        public string type;
        public List<Message> message;
        public string event_id;
    }
}