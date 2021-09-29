using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra2
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublicationDate { get; set; }
        public string ISBN { get; set; }
        public string Thumbnail { get; set; }
        public int PageCount { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
    }
}
