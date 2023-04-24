using System;

namespace HashApiApp.Models
{
    public class Hash
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Sha1 { get; set; }
        public int Count { get; internal set; }
    }
}
