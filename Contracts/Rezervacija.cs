using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public int IdKoncerta { get; set; }
        public DateTime VremeRezervacije { get; set; }
        public int KolicinaKarata { get; set; }
        public StanjeRezervacije State { get; set; }

    }
}
