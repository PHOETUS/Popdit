namespace PopditWebApi
{
    using System;

    public partial class Category
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string GoogleDescription { get; set; }
        public Nullable<int> CategoryId { get; set; }
    }
}
