using System;

namespace hospitableBot.Models
{
    public class LoginResponse
    {
        public Data data { get; set; }
        public bool errors { get; set; }
        public string message { get; set; }
        public class Team
        {
            public int id { get; set; }
            public string owner_email { get; set; }
            public string owner_first_name { get; set; }
            public string owner_last_name { get; set; }
            public string owner_picture { get; set; }
        }

        public class Data
        {
            public string id { get; set; }
            public string id_hash { get; set; }
            public string intercom_hash { get; set; }
            public string name { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string company { get; set; }
            public string timezone { get; set; }
            public int timezone_offset { get; set; }
            public string phone { get; set; }
            public string picture { get; set; }
            public bool uses_24_hour_format { get; set; }
            public object week_starts_on { get; set; }
            public bool beta_features_enabled { get; set; }
            public DateTime created_at { get; set; }
            public bool has_not_started_connecting_channels { get; set; }
            public bool should_use_airbnb_official { get; set; }
            public int channels { get; set; }
            public int connected_channels { get; set; }
            public bool autohost_enabled { get; set; }
            public Team team { get; set; }
            public string token { get; set; }
        }

    }
}