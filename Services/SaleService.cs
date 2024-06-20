using Models;
using Models.Utils;
using Repositories;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;

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

        public async Task<SaleDTO> Post(SaleDTO saleDTO)
        {
            Sale sale = new Sale(saleDTO);
            sale.Flight = returnFlight(saleDTO.Flight);
            sale.Passengers = returnPassagers(saleDTO.Passengers);

            if (ValidCPF(sale.Passengers) && NotDuplicateCPF(sale) && validFlight(sale.Flight) && validCount(sale.Flight.Id, sale.Passengers.Count))
            {
                _saleRepository.Post(sale);

                return saleDTO;
            } 
            return null;
        }

        private List<Passenger> returnPassagers(List<string> passengers)
        {
            string baseUri = "https://localhost:7298/";
            List<Passenger> passengersList = new List<Passenger>();

            foreach (var passenger in passengers)
            {
                string requestUri = $"/api/Passengers/{passenger}/";
                var p = ApiConsume<Passenger>.Get(baseUri, requestUri).Result;

                passengersList.Add(p);
            }

            return passengersList;

        }

        private Flight returnFlight(int flight)
        {
            string baseUri = "https://localhost:7258";
            string requestUri = $"/api/Flights/{flight}";

            var flightReturn = ApiConsume<Flight>.Get(baseUri, requestUri).Result;
            flightReturn.Id = flight;
            return flightReturn;
        }

        private bool NotDuplicateCPF(Sale sale)
        {
            HashSet<string> cpfLists = new HashSet<string>();

            var ListSale = _saleRepository.GetSale().Result;


            var filteredSales = ListSale.Where(s => s.Flight.Id == sale.Flight.Id).ToList();


            foreach (var passenger in sale.Passengers.Concat(filteredSales.SelectMany(s => s.Passengers)))
            {
                if (!cpfLists.Add(passenger.CPF))
                {
                    return false;
                }
            }

            return true;
        }

        private bool validCount(int id, int count)
        {
            var flightGet = returnFlight(id);


            if (flightGet.Sales < count)
            {
                return false;
            }

            _saleRepository.UpdateFlight(flightGet.Id, count, "Sell");
            return true;
        }

        


        private bool validFlight(Flight flight)
        {
            string baseUri = "https://localhost:7258/";
            string requestUri = $"/api/Flights/{flight.Id}";

            var flightGet = ApiConsume<Flight>.Get(baseUri, requestUri).Result;

            if(flightGet != null ) {

                return true;

            }

            return false;
        }

        public bool ValidCPF(List<Passenger> passengers)
        {
            string baseUri = "https://localhost:7298";
            string requestUri = $"/api/Passengers";

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
                if (!listPassengers.Any(p => p.CPF == passenger.CPF) || passenger.Status == true)
                {
                    return false;
                }
            }

            return true;
        }

        public Task<bool> SoldSale(int id)
        {
            return _saleRepository.SoldSale(id);
        }

        public void RemoveSale(int id)
        {
            new SaleRepository().RemoveSale(id);
        }

    }
}
