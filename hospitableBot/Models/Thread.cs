using System;
using System.Collections.Generic;

namespace hospitableBot.Models
{
    public class Thread
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string GuestName { get; set; }
        public string Platform { get; set; }
        public string ThreadId { get; set; }
        public string ReservationCode { get; set; }
        public string Phone { get; set; }
        public List<Message> Messages { get; set; }
    }
}