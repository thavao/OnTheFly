using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SaleDTO
    {
        public int Flight { get; set; }
        //public int FlightId { get; set; }
        public List<string> Passengers { get; set; }
        //public string CpfBuyer { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }
    }
}
