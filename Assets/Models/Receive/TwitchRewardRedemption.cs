using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class TwitchRewardRedemption
    {
        public Guid id;
        public TwitchUser user;
        public int channel_id;
        public string redeemed_at;
        public TwitchReward reward;
        public string status;
    }
}
