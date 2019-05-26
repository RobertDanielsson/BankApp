using BankApp.Application.Interfaces;
using System;

namespace BankApp.Infrastructure
{
    public class SystemClock : IDateTime
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
