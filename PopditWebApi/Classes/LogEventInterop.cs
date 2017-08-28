namespace PopditWebApi
{
    public class LogEventInterop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
    }
}
