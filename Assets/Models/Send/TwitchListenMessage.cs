using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    [System.Serializable]
    public class TwitchListenMessage
    {
        public string type = "LISTEN";
        public string nonce;
        public TwitchListenMessageData data;
    }
}
