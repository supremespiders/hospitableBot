using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using hospitableBot.Helpers;
using hospitableBot.Models;

namespace hospitableBot.Services
{
    public class HospitableClient
    {
        private readonly HttpClient _httpClient;
        private string _token;
        private readonly string _user;
        private readonly string _pass;
        private readonly int _delay;
        private DateTime _from;
        private DateTime _to;
        private readonly string _delimiter;
        private string _outputFolder;

        public HospitableClient(string user, string pass, int delay, DateTime from, DateTime to, string delimiter, string outputFolder)
        {
            _user = user;
            _pass = pass;
            _delay = delay;
            _from = from;
            _to = to;
            _delimiter = delimiter;
            _outputFolder = outputFolder;
            _httpClient = new HttpClient(new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
        }

        public async Task MainWork(List<string> ids)
        {
            await LoginIfNeeded();
            //var msgs = await GetMessages("3ed489e4-96e8-4218-989d-164202b8f398");
            // await GetConversation();
            //await GetProperties();
            //var threads = await GetReservations("2017-05-30", "2021-06-30", new List<string>() { "641518" });
            foreach (var id in ids)
            {
                var threads = await GetReservations(_from.ToString("yyyy-MM-dd"), _to.ToString("yyyy-MM-dd"), new List<string>() { id });
                for (var i = 0; i < threads.Count; i++)
                {
                    var thread = threads[i];
                    Notifier.Display($"Getting conversations of thread {i + 1} / {threads.Count}");
                    Notifier.Progress(i + 1, threads.Count);
                    await Task.Delay(1000 * _delay);
                    var messages = await GetMessages(thread.ThreadId);
                    thread.Messages = messages;
                }

                var deliminator = "~";
                var sb = new StringBuilder($"Property ID{deliminator}Property Name{deliminator}Source{deliminator}Reservation Start Date{deliminator}Reservation End Date{deliminator}Guest Name{deliminator}Thread Id{deliminator}From{deliminator}Message Date{deliminator}Message\r\n");
                var outputs = new List<Output>();
                foreach (var thread in threads)
                {
                    foreach (var threadMessage in thread.Messages)
                    {
                        outputs.Add(new Output()
                        {
                            PropertyId = thread.PropertyId.ToString(),
                            PropertyName = thread.PropertyName,
                            Source = thread.Platform,
                            ThreadId = thread.ThreadId,
                            GuestName = thread.GuestName,
                            CheckIn = thread.CheckIn.ToString("MM/dd/yyyy"),
                            Checkout = thread.CheckOut.ToString("MM/dd/yyyy HH:mm"),
                            From = threadMessage.SenderName,
                            MessageDate = threadMessage.TimeStamp.ToString("MM/dd/yyyy HH:mm tt") + " MST",
                            Message = threadMessage.Body,
                            MessageResponseTime = threadMessage.MessageResponseTime
                        });
                        sb.AppendLine($"{thread.PropertyId}{deliminator}{thread.PropertyName}{deliminator}{thread.Platform}{deliminator}{thread.CheckIn:MM/dd/yyyy}{deliminator}{thread.CheckOut}{deliminator}{thread.GuestName}{deliminator}{thread.ThreadId}{deliminator}{threadMessage.SenderName}{deliminator}{threadMessage.TimeStamp} MST{deliminator}\"{threadMessage.Body}\"");
                    }
                }

                using (var writer = new StreamWriter(_outputFolder + $"/{id}_{_from:yyyy-MM-dd}_{_to:yyyy-MM-dd}.csv"))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = _delimiter
                }))
                {
                    await csv.WriteRecordsAsync(outputs);
                }

                // File.WriteAllText("output.csv", sb.ToString());
            }


            Notifier.Display("Completed");
        }

        private async Task<List<Message>> GetMessages(string thread)
        {
            var messagesResponse = (await Get($"https://api.hospitable.com/v1/threads/{thread}/messages")).ToObject<MessagesResponse>();
            var messages = new List<Message>();

            for (var i = 0; i < messagesResponse.data.Count; i++)
            {
                var datum = messagesResponse.data[i];
                if (datum.message == null) continue;
                var gap = "";
                if (i != messagesResponse.data.Count - 1)
                {
                    var t = ((int)(datum.created - messagesResponse.data[i + 1].created).TotalMinutes);
                    if (t < 999999)
                        gap = t.ToString();
                }

                var mountain = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(datum.created, "Mountain Standard Time");
                messages.Add(new Message
                {
                    Body = datum.message.Replace("`", "'").Replace("’", "").Replace("\"", "''"),
                    TimeStamp = mountain,
                    SenderName = datum.from?.name,
                    SenderPicture = datum.from?.picture_url,
                    MessageResponseTime = gap
                });
            }

            return messages;
        }

        private async Task<List<Thread>> GetReservations(string startDate, string endDate, List<string> propertiesIds)
        {
            Notifier.Display($"Getting reservations for {propertiesIds[0]} between {startDate} and {endDate}");
            var reservationsResponse = (await Get($"https://api.hospitable.com/v1/reservations/?starts_or_ends_between={startDate}_{endDate}&timezones=false&property_ids={string.Join(",", propertiesIds)}&confirmed=true")).ToObject<ReservationsResponse>();
            // var reservationsResponse = (await Get($"https://api.hospitable.com/v1/reservations/?starts_or_ends_between={startDate}_{endDate}&timezones=false&property_ids=641518,641524,642748,642758,642754,642746,641526,642766,642764,642750,642756,642760,642744,642752,641522,641516,641520,665110&confirmed=true")).ToObject<ReservationsResponse>();
            var threads = new List<Thread>();
            foreach (var datum in reservationsResponse.data)
            {
                threads.Add(new Thread()
                {
                    Platform = datum.platform,
                    GuestName = datum.guest.Trim(),
                    CheckIn = datum.checkin,
                    CheckOut = datum.checkout,
                    Phone = datum.phone,
                    PropertyId = datum.listing.property_id,
                    PropertyName = datum.listing.property_name,
                    ThreadId = datum.thread_uuid,
                    ReservationCode = datum.code,
                    Messages = new List<Message>()
                });
            }

            Notifier.Display($"Found {threads.Count} threads");
            return threads;
        }

        public async Task<PropertiesResponse> GetProperties(int limit = 50)
        {
            Notifier.Display("Getting all properties");
            var propertiesResponse = (await PostJson($"https://api.hospitable.com/v1/properties?limit={limit}", "{\"filters\":[{\"id\":\"properties-unlisted\",\"value\":\"include_stealth\",\"inverted\":false}]}")).ToObject<PropertiesResponse>();
            Notifier.Display($"Found {propertiesResponse.data.Count} properties");
            return propertiesResponse;
        }

        private async Task<Thread> GetConversation(string threadId)
        {
            var threadResponse = (await Get($"https://api.hospitable.com/v1/threads/{threadId}")).ToObject<ThreadResponse>();
            var thread = new Thread
            {
                GuestName = threadResponse.data.guest.first_name + " " + threadResponse.data.guest.last_name,
                CheckIn = DateTime.Parse(threadResponse.data.checkin),
                CheckOut = DateTime.Parse(threadResponse.data.checkout),
                Platform = threadResponse.data.platform
            };
            return thread;
        }

        private async Task<string> Get(string url)
        {
            var json = await _httpClient.GetHtml(url, 1, new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + _token }
            });
            return json;
        }

        private async Task<string> PostJson(string url, string json)
        {
            var r = await _httpClient.PostJson(url, json, 1, new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + _token }
            });
            if (r.Contains("{\"error\":\"You must be logged in to perform this action.\"}"))
            {
                throw new KnownException("Not logged in");
            }

            return r;
        }

        public async Task LoginIfNeeded()
        {
            if (!File.Exists("ses"))
            {
                await Login();
                return;
            }

            Notifier.Display("Verifying if session still valid");
            _token = File.ReadAllText("ses");
            try
            {
                await PostJson($"https://api.hospitable.com/v1/properties?limit=1", "{\"filters\":[{\"id\":\"properties-unlisted\",\"value\":\"include_stealth\",\"inverted\":false}]}");
            }
            catch (Exception e)
            {
                await Login();
            }
        }

        private async Task Login()
        {
            Notifier.Display("Logging in");
            var loginResponse = await _httpClient.PostJson<LoginResponse>("https://api.hospitable.com/v1/auth/login", "{\"email\":\"" + _user + "\",\"password\":\"" + _pass + "\"}");
            if (loginResponse.errors)
                throw new KnownException($"Failed to login : {loginResponse.message}");
            _token = loginResponse.data?.token;
            if (_token == null)
                throw new KnownException($"Failed to retrieve token");
            Notifier.Display("Logged in");
            File.WriteAllText("ses", _token);
        }
    }
}