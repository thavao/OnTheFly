using Models;
using Models.Utils;
using Repositories;
using System.Security;

namespace Services
{
    public class SaleService
    {
        private SaleRepository _saleRepository;

        public SaleService()
        {
            _saleRepository = new();
        }

        public async Task<List<Sale>> GetSale()
        {
            return await _saleRepository.GetSale();
        }

        public async Task<Sale> GetSale(int id)
        {
            return await _saleRepository.GetSale(id);
        }

        public Sale Post(Sale sale)
        {
            if (ValidCPF(sale.Passengers) && validFlight(sale.Flight) )
            {
                return _saleRepository.Post(sale);

            }
            return null;
        }

        private bool validFlight(Flight flight)
        {
            string baseUri = "https://localhost:7034/";
            string requestUri = $"GetFlights/{flight.Id}";

            var flightGet = ApiConsume<Flight>.Get(baseUri, requestUri).Result;

            if(flightGet != null ) {

                return true;

            }

            return false;
        }

        public bool ValidCPF(List<Passenger> passengers)
        {
            string baseUri = "https://localhost:7034/";
            string requestUri = $"GetPassengers";

            var listPassengers = ApiConsume<List<Passenger>>.Get(baseUri, requestUri).Result;

            var firstPassenger = passengers.FirstOrDefault();
            if (firstPassenger != null)
            {
                var idade = DateTime.Today.Year - firstPassenger.DtBirth.Year;
                if (firstPassenger.DtBirth > DateTime.Today.AddYears(-idade))
                {
                    idade--;
                }

                if (idade < 18)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            foreach (var passenger in passengers)
            {
                if (!listPassengers.Any(p => p.CPF == passenger.CPF))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
