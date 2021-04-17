using System;
using System.Collections.Generic;

namespace Domain.Frames.Endpoint
{
    public class MultiItemFrame<T>
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public int TotalResults { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}