namespace PopditiOS.Models
{
    using System;

    public partial class EventMobile
    {
        public string ProviderName { get; set; }  // Added for mobile app.
        public string Msg { get; set; }  // Added for mobile app.
        public int Id { get; set; }
        //public int ProfileId { get; set; }
        public int BubbleId { get; set; }
        public string TimestampJson
        {
            get { return String.Format("{0:yyyy-MM-dd HH:mm:ss}", Timestamp); }
            set { Timestamp = DateTime.Parse(value); }
        }
        private DateTime Timestamp { get; set; }
    }
}
