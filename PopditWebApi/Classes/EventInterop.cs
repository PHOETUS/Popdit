using System;

namespace PopditWebApi
{
    public partial class EventInterop
    {
        public int Id { get; set; }
        //public int ProfileId { get; set; }
        //public int BubbleId { get; set; }
        public string ProviderName { get; set; }
        public string MsgTitle { get; set; }
        public string Msg { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimestampJson // ... at this time.
        {
            get { return String.Format("{0:yyyy-MM-dd HH:mm:ss}", Timestamp); }
            set { Timestamp = DateTime.Parse(value); }
        }
        public string TimestampToday
        {
            get { return String.Format("{0:h:mm tt}", Timestamp); }
        }
        private System.DateTime Timestamp { get; set; }
    }
}
