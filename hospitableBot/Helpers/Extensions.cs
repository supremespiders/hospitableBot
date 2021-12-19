using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using hospitableBot.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace hospitableBot.Helpers
{
    public static class Extensions
    {
        public static async Task<string> HandleAndRepeat(this HttpClient httpClient, HttpRequestMessage req, int maxAttempts = 1, CancellationToken ct = new CancellationToken())
        {
            int tries = 0;
            do
            {
                try
                {
                    var r = await httpClient.SendAsync(req, ct).ConfigureAwait(false);
                    var s = await r.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return (s);
                }
                catch (WebException ex)
                {
                    var errorMessage = "";
                    try
                    {
                        errorMessage = await new StreamReader(ex.Response.GetResponseStream()).ReadToEndAsync();
                    }
                    catch (Exception)
                    {
                        //
                    }

                    tries++;
                    if (tries == maxAttempts)
                    {
                        throw new KnownException($"Error calling : {req.RequestUri}\n{ex.Message} {errorMessage}");
                    }

                    await Task.Delay(2000, ct).ConfigureAwait(false);
                }
            } while (true);
        }

        public static async Task<T> PostJson<T>(this HttpClient httpClient, string url, string json, int maxAttempts = 1, Dictionary<string, string> headers = null, CancellationToken ct = new CancellationToken())
        {
            return JsonConvert.DeserializeObject<T>(await httpClient.PostJson(url, json, maxAttempts, headers, ct));
        }
        public static async Task<string> PostJson(this HttpClient httpClient, string url, string json, int maxAttempts = 1, Dictionary<string, string> headers = null, CancellationToken ct = new CancellationToken())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (content.Headers.ContentType != null)
                content.Headers.ContentType.CharSet = "";
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };

            if (headers == null)
                return await httpClient.HandleAndRepeat(req, maxAttempts, ct);

            foreach (var header in headers)
                req.Headers.Add(header.Key, header.Value);

            return await httpClient.HandleAndRepeat(req, maxAttempts, ct);
        }

        public static async Task<string> PostFormData(this HttpClient httpClient, string url, Dictionary<string, string> data, int maxAttempts = 1, Dictionary<string, string> headers = null, CancellationToken ct = new CancellationToken())
        {
            var content = new FormUrlEncodedContent(data);
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };

            if (headers == null)
                return await httpClient.HandleAndRepeat(req, maxAttempts, ct);

            foreach (var header in headers)
                req.Headers.Add(header.Key, header.Value);

            return await httpClient.HandleAndRepeat(req, maxAttempts, ct);
        }

        public static T ToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public static async Task<string> GetHtml(this HttpClient httpClient, string url, int maxAttempts = 1, Dictionary<string, string> headers = null, CancellationToken ct = new CancellationToken())
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if (headers == null)
                return await httpClient.HandleAndRepeat(req, maxAttempts, ct);

            foreach (var header in headers)
                req.Headers.Add(header.Key, header.Value);

            return await httpClient.HandleAndRepeat(req, maxAttempts, ct);
        }

        public static HtmlDocument ToDoc(this string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
        public static string GetStringBetween(this string text, string start, string end)
        {
            var p1 = text.IndexOf(start, StringComparison.Ordinal) + start.Length;
            if (p1 == start.Length - 1) return null;
            var p2 = text.IndexOf(end, p1, StringComparison.Ordinal);
            if (p2 == -1) return null;
            return end == "" ? text.Substring(p1) : text.Substring(p1, p2 - p1);
        }

        public static T Deserialize<T>(this string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        // public static async Task<List<Input>> ReadInputs(this string fileName)
        // {
        //     if (!File.Exists(fileName))
        //         throw new KnownException($"File {fileName} not found");
        //     var lines = (await File.ReadAllLinesAsync(fileName)).Distinct().ToList();
        //     lines.Remove("");
        //     var inputs = lines.Select(x =>
        //     {
        //         var c = x.Split(",");
        //         return new Input { Username = c[0], Password = c[1] };
        //     }).ToList();
        //     return inputs;
        // }
    }
}