namespace PopditMobileApi
{
    using System;

    public partial class EventMobile
    {
        // Fields from the DB.
        public int Id { get; set; } // Event ID        
        public int ProfileId { get; set; } // This person ...
        public int BubbleId { get; set; } // ... popped this bubble ...
        public string TimestampJson // ... at this time.
        {
            get { return String.Format("{0:yyyy-MM-dd HH:mm:ss}", Timestamp); }
            set { Timestamp = DateTime.Parse(value); }
        }
        public string TimestampToday
        {
            get {  return String.Format("{0:h:mm tt}", Timestamp); }
        }
        public DateTime Timestamp { get; set; }

        // Mobile app display fields.
        public string ProviderName { get; set; }
        public string MsgTitle { get; set; }
        public string Msg { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Suppressed { get; set; }
    }
}
