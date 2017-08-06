namespace PopditWebApi
{
    using System;
    using System.Runtime.Serialization;

    public partial class FilterInterop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> ScheduleId { get; set; }
        public int RadiusId { get; set; }
        public bool Active { get; set; }
        public string PublicKey { get; set; }
    }
}
