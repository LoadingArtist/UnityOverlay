using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    [System.Serializable]
    public class TwitchListenMessageData
    {
        public List<string> topics;
        public string auth_token;
    }
}
