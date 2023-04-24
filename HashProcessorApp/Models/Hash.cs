using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashProcessorApp.Models
{
    public class Hash
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Sha1 { get; set; }
    }
}
