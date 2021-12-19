using System;
using System.Collections.Generic;

namespace hospitableBot.Models
{
    public class ReservationsResponse
    {
        public List<Datum> data { get; set; }

        public class Listing
        {
            public int property_id { get; set; }
            public string property_name { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public List<string> tags { get; set; }
        }

        public class Autohost
        {
            public object id { get; set; }
            public object status { get; set; }
            public object supervised { get; set; }
        }

        public class Links
        {
            public string accept { get; set; }
            public string reject { get; set; }
            public string extended { get; set; }
        }

        public class Datum
        {
            public string uuid { get; set; }
            public string listing_id { get; set; }
            public string thread_id { get; set; }
            public string thread_uuid { get; set; }
            public string user_id { get; set; }
            public string guest_id { get; set; }
            public string code { get; set; }
            public string platform { get; set; }
            public string channel { get; set; }
            public bool manual { get; set; }
            public bool editable { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public bool is_booking_request { get; set; }
            public string status { get; set; }
            public bool actionable { get; set; }
            public DateTime checkin { get; set; }
            public string checkin_label { get; set; }
            public string checkin_tooltip { get; set; }
            public DateTime checkout { get; set; }
            public string checkout_label { get; set; }
            public string checkout_tooltip { get; set; }
            public DateTime start_date { get; set; }
            public DateTime end_date { get; set; }
            public string timezone { get; set; }
            public string currency { get; set; }
            public string guest { get; set; }
            public string guest_picture { get; set; }
            public string guest_thumbnail { get; set; }
            public Listing listing { get; set; }
            public Autohost autohost { get; set; }
            public string currency_code { get; set; }
            public int num_adults { get; set; }
            public int num_children { get; set; }
            public int num_infants { get; set; }
            public int num_pets { get; set; }
            public DateTime created_at { get; set; }
            public string created_at_label { get; set; }
            public bool this_thread { get; set; }
            public string entity_type { get; set; }
            public string note { get; set; }
            public Links _links { get; set; }
        }
    }
}