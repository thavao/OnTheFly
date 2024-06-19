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

        public Sale Post(Sale sale)
        {
            if (ValidCPF(sale.Passengers) && validFlight(sale.Flight) && validCount(sale.Flight.Id, sale.Passengers.Count) )
            {
                return _saleRepository.Post(sale);

            } 
            return null;
        }

        private bool validCount(int id, int count)
        {
            string baseUri = "https://localhost:7034/";
            string requestUri = $"GetFlights/{id}";

            var flightGet = ApiConsume<Flight>.Get(baseUri, requestUri).Result;
           
            if(flightGet.Sales < count)
            {
                return false;
            }

            UpdateFlight(flightGet, count);
            return true;
        }

        private async Task UpdateFlight(Flight flightGet, int count)
        {
            string baseUri = "https://localhost:7034/";
            string requestUri = $"UpdateFlight/{flightGet.Id}";

            var updatedFlight = new
            {
                Id = flightGet.Id,
                Sales = flightGet.Sales - count,
                Status = flightGet.Status
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(updatedFlight);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                using HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PutAsync(baseUri + requestUri, content);

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
                if (!listPassengers.Any(p => p.CPF == passenger.CPF))
                {
                    return false;
                }
            }

            return true;
        }

        public Task<bool> Put(Sale sale)
        {
            return _saleRepository.PutSale(sale);
        }
        public Task<bool> SoldSale(int id)
        {
            return _saleRepository.SoldSale(id);
        }

    }
}
