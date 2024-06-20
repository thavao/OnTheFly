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
        public static readonly string GetPassengersById = "Select CpfPassenger FROM PassengerSale WHERE SaleId = @SaleId";

        public int Id { get; set; }
        public Flight Flight { get; set; }
        //public int FlightId { get; set; }
        public List<Passenger> Passengers { get; set; }
        //public string CpfBuyer { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }

        public Sale() { }

        public Sale(SaleDTO dto)
        {
            Flight = new Flight { Id = dto.Flight };
            Passengers = dto.Passengers?.Select(p => new Passenger { CPF = p }).ToList() ?? new List<Passenger>();
            Reserved = true;
            Sold = false;

        }
    }
}
