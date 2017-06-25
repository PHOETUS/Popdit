namespace PopditMobile.iOS
{
    using System;

    public partial class EventMobile
    {
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
