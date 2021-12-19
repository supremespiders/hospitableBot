using System;

namespace hospitableBot.Models
{
    public class Message
    {
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
        public string SenderName { get; set; }
        public string SenderPicture { get; set; }
        public string MessageResponseTime { get; set; }
    }
}