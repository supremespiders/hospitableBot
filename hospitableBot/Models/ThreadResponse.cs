using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace hospitableBot.Models
{
    public class ThreadResponse
    {
        public Data data { get; set; }

        public class Overall
        {
            public int count { get; set; }
            public int average { get; set; }
        }

        public class Communications
        {
            public int count { get; set; }
            public int average { get; set; }
        }

        public class RespectsHouseRules
        {
            public int count { get; set; }
            public int average { get; set; }
        }

        public class Cleanliness
        {
            public int count { get; set; }
            public int average { get; set; }
        }

        public class Ratings
        {
            public Overall overall { get; set; }
            public Communications communications { get; set; }
            public RespectsHouseRules respects_house_rules { get; set; }
            public Cleanliness cleanliness { get; set; }
        }

        public class Review
        {
            public string author { get; set; }
            public string image { get; set; }
            public object rating { get; set; }
            public string comment { get; set; }
            public object response { get; set; }
            public DateTime date { get; set; }
        }

        public class Guest
        {
            public string uuid { get; set; }
            public DateTime account_created_at { get; set; }
            public string guest_id { get; set; }
            public string picture { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string email_anonymous { get; set; }
            public string phone { get; set; }
            public string timezone { get; set; }
            public string about { get; set; }
            public bool verified { get; set; }
            public string locale { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string ui_location { get; set; }
            public string address { get; set; }
            public int trip_count { get; set; }
            public Ratings ratings { get; set; }
            public List<Review> reviews { get; set; }
            public string profile_url { get; set; }
            public string platform { get; set; }
            public string platform_human { get; set; }
            public object custom_first_name { get; set; }
            public object custom_last_name { get; set; }
        }

        public class Host
        {
            public string name { get; set; }
            public string image { get; set; }
            public string user_id { get; set; }
        }

        public class Listing
        {
            public string page_url { get; set; }
            public List<object> tags { get; set; }
        }

        public class Language
        {
            public string key { get; set; }
            public string label { get; set; }
        }

        public class Host2
        {
            public string name { get; set; }
            public string image { get; set; }
            public string user_id { get; set; }
        }

        public class Links
        {
            [JsonProperty("mark-as-read")] public string MarkAsRead { get; set; }
            public string chat { get; set; }
            public string messages { get; set; }
            public string activity { get; set; }
            public string events { get; set; }
            public string reservations { get; set; }
        }

        public class Data
        {
            public string uuid { get; set; }
            public string thread_id { get; set; }
            public string last_message { get; set; }
            public DateTime last_message_received_at { get; set; }
            public DateTime last_message_at { get; set; }
            public string last_message_at_label { get; set; }
            public string advanced_status { get; set; }
            public string checkin { get; set; }
            public string checkout { get; set; }
            public string status_key { get; set; }
            public string status { get; set; }
            public string reservation_code { get; set; }
            public object actionable_reservation_uuid { get; set; }
            public int total { get; set; }
            public string currency { get; set; }
            public string source { get; set; }
            public bool returning_guest { get; set; }
            public Guest guest { get; set; }
            public string chances_of_booking { get; set; }
            public int adults { get; set; }
            public int children { get; set; }
            public int nights { get; set; }
            public bool unread { get; set; }
            public List<Host> hosts { get; set; }
            public Listing listing { get; set; }
            public string note { get; set; }
            public bool resolved { get; set; }
            public bool starred { get; set; }
            public bool archived { get; set; }
            public string distance { get; set; }
            public Language language { get; set; }
            public Host host { get; set; }
            public string platform { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public Links _links { get; set; }
        }
    }
}