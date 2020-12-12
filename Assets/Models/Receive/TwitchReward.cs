using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class TwitchReward
    {
        public Guid id;
        public string channel_id;
        public string title;
        public string prompt;
        public int cost;
        public bool is_user_input_required;
        public bool is_sub_only;
        public string image;
        public object default_image;
        public string background_color;
        public bool is_enabled;
        public bool is_paused;
        public bool is_in_stock;
        public object max_per_stream;
        public bool should_redemptions_skip_request_queue;
        public string template_id;
        public string updated_for_indicated_at;
        public object max_per_user_per_stream;
        public object global_cooldown;
        public string redemptions_redeemed_current_stream;
        public string cooldown_expires_at;
    }
}
