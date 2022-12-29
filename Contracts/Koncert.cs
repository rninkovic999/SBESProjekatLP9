using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Koncert
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime VremePocetka { get; set; }
        public string Lokacija  { get; set; }
        public double cenaKarte { get; set; }

    }
}
