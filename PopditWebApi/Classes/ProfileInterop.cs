namespace PopditWebApi
{
    using System;

    public partial class ProfileInterop
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string CallbackAddress { get; set; }
        public Nullable<int> RadiusId { get; set; }
        public string DobJson
        {
            get { return String.Format("{0:yyyy-MM-dd}", DOB); }
            set { DOB = DateTime.Parse(value); }
        }
        private Nullable<System.DateTime> DOB { get; set; }
        public Nullable<bool> Male { get; set; }
    }
}
