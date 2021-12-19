using System.Collections.Generic;

namespace hospitableBot.Models
{
    public class PropertiesResponse
    {
        public List<Datum> data { get; set; }
        public Meta meta { get; set; }

        public class Counts
        {
            public int airbnb { get; set; }
            public int homeaway { get; set; }
            public int booking { get; set; }
            public int total { get; set; }
        }

        public class Host
        {
            public string platform_id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string name { get; set; }
            public string picture { get; set; }
            public string platform { get; set; }
        }

        public class Location
        {
            public string apt { get; set; }
            public string street { get; set; }
            public string city { get; set; }
            public string zipcode { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string country_name { get; set; }
        }

        public class Photo
        {
            public string caption { get; set; }
            public string xx_large { get; set; }
            public string thumbnail { get; set; }
            public int sort_order { get; set; }
        }

        public class Datum
        {
            public int id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string currency { get; set; }
            public bool listed { get; set; }
            public bool is_new { get; set; }
            public string timezone { get; set; }
            public string picture { get; set; }
            public string thumbnail { get; set; }
            public List<string> tags { get; set; }
            public List<string> types { get; set; }
            public Counts counts { get; set; }
            public List<Host> hosts { get; set; }
            public bool sync { get; set; }
            public bool stealth { get; set; }
            public bool heartbeats { get; set; }
            public Location location { get; set; }
            public bool muted { get; set; }
            public string summary { get; set; }
            public string space { get; set; }
            public string access { get; set; }
            public string neighborhood_overview { get; set; }
            public string transit { get; set; }
            public string notes { get; set; }
            public int person_capacity { get; set; }
            public List<string> languages { get; set; }
            public List<string> amenities { get; set; }
            public string property_type { get; set; }
            public int beds { get; set; }
            public List<Photo> photos { get; set; }
            public string street { get; set; }
            public string zip { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public int bedrooms { get; set; }
            public int bathrooms { get; set; }
            public object kitchens { get; set; }
            public object living_rooms { get; set; }
            public object offices { get; set; }
            public object gardens { get; set; }
            public object patios { get; set; }
            public object pools { get; set; }
            public object hot_tubs { get; set; }
            public object garages { get; set; }
            public object gyms { get; set; }
            public string check_in_time { get; set; }
            public string check_out_time { get; set; }
            public string house_rules { get; set; }
            public string public_name { get; set; }
            public string calendar_cleanup_job_id { get; set; }

            public override string ToString()
            {
                return name;
            }
        }

        public class PageNumbers
        {
            public int total_pages { get; set; }
            public int this_page { get; set; }
        }

        public class Links
        {
            public string first { get; set; }
            public string last { get; set; }
            public object next { get; set; }
            public object previous { get; set; }
        }

        public class Pagination
        {
            public int total { get; set; }
            public int this_page { get; set; }
            public int per_page { get; set; }
            public int offset { get; set; }
            public PageNumbers page_numbers { get; set; }
            public Links _links { get; set; }
        }

        public class Meta
        {
            public Pagination pagination { get; set; }
        }
    }
}