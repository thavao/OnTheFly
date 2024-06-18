using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Sale
    {
        public static readonly string Get = @"SELECT Id, FlightId, CpfBuyer, Reserved, Sold FROM Sale";

        public static readonly string GetId = Get + " WHERE Id = @Id";
        public static readonly string GetPassengers = "Select SaleId, CpfPassenger FROM PassengerSale";

        public int Id { get; set; }
        public Flight Flight { get; set; }
        public List<Passenger> Passengers { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }
    }
}
