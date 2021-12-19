using System;

namespace hospitableBot.Models
{
    public class KnownException : Exception
    {
        public KnownException(string s) : base(s)
        {
        }
    }
}