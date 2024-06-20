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
            string baseUri = "https://localhost:7034/";
            List<Passenger> passagersList = new List<Passenger>();

            foreach (var passenger in passengers)
            {
                string requestUri = $"GetPassengers/{passenger}";
                var p = ApiConsume<Passenger>.Get(baseUri, requestUri).Result;

                passagersList.Add(p);
            }

            return passagersList;

        }

        private Flight returnFlight(int flight)
        {
            string baseUri = "https://localhost:7034/";
            string requestUri = $"GetFlights/{flight}";

            return ApiConsume<Flight>.Get(baseUri, requestUri).Result;
          

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

            UpdateFlight(flightGet, count);
            return true;
        }

        private async Task UpdateFlight(Flight flightGet, int count)
        {
            string baseUri = "https://localhost:7034/";
            string requestUri = $"UpdateFlight/{flightGet.Id}/{count}";

            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PatchAsync(baseUri + requestUri, null);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Atualização do voo bem-sucedida!");
                }
                else
                {
                    Console.WriteLine($"Erro ao atualizar o voo. Código de status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o voo: {ex.Message}");
            }
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
                if (!listPassengers.Any(p => p.CPF == passenger.CPF) ||passenger.Status == false)
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
