using System;

namespace Domain.Frames
{
    public class SingleItemFrame<T>
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public T Result { get; set; }
    }
}