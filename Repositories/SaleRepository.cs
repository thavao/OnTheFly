using Dapper;
using Microsoft.Data.SqlClient;
using Models;
using Models.Utils;

namespace Repositories
{
    public class SaleRepository
    {
        private readonly string _conn;
        private readonly string _passengerUri;
        private readonly string _flightUri;

        public SaleRepository()
        {
            _conn = "Data Source=127.0.0.1; Initial Catalog=DbSales; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";

            _passengerUri = "https://localhost:7298";
            _flightUri = "https://localhost:7258";
        }

        public async Task<List<Sale>> GetSale()
        {
            var list = new List<Sale>();
            using var connection = new SqlConnection(_conn);
            connection.Open();

            var t1 = ApiConsume<List<Passenger>>.Get(_passengerUri, "/api/Passengers");
            var t2 = ApiConsume<List<Flight>>.Get(_flightUri, "/api/Flights");
            var t3 = connection.QueryAsync<dynamic>(Sale.GetPassengers);

            await Task.WhenAll(t1, t2, t3);

            var passengersList = t1.Result;
            var flightsList = t2.Result;
            var passengersAsDynamic = t3.Result;


            if (passengersList == null)
                return null;

            foreach (dynamic row in connection.Query<dynamic>(Sale.Get).ToList())
            {
                Sale sale = new()
                {
                    Id = row.Id,
                    Reserved = row.Reserved,
                    Sold = row.Sold,
                    Passengers = new(),
                    Flight = flightsList.FirstOrDefault(f => f.Id == row.FlightId)
                };


                foreach (var passengerRow in passengersAsDynamic)
                {
                    if (passengerRow.SaleId == sale.Id)
                    {
                        string cpf = passengerRow.CpfPassenger;
                        var p = passengersList.FirstOrDefault(p => p.CPF.Equals(cpf));
                        sale.Passengers.Add(p);
                    }
                }

                list.Add(sale);
            }
            return list;
        }


        public async Task<Sale> GetSale(int id)
        {
            var list = new List<Sale>();
            using var connection = new SqlConnection(_conn);
            connection.Open();

            dynamic? row = connection.Query<dynamic>(Sale.GetId, new { Id = id }).FirstOrDefault();

            if (row == null)
                return null;

            var t1 = ApiConsume<List<Passenger>>.Get(_passengerUri, "/api/Passengers");
            var t2 = ApiConsume<Flight>.Get(_flightUri, $"/api/Flights/{row.FlightId}");
            var t3 = connection.QueryAsync<string>(Sale.GetPassengersById, new { SaleId = id });

            await Task.WhenAll(t1, t2, t3);

            var listPassenger = t1.Result;
            var flight = t2.Result;
            var cpfs = t3.Result;

            if (listPassenger == null || flight == null)
                return null;

            listPassenger = listPassenger.Where(p => cpfs.Contains(p.CPF)).ToList();

            Sale sale = new()
            {
                Id = row.Id,
                Reserved = row.Reserved,
                Sold = row.Sold,
                Passengers = listPassenger,
                Flight = flight
            };

            return sale;
        }

        public Sale Post(Sale sale)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();

                string insertSaleQuery = @"INSERT INTO Sale (FlightId, CpfBuyer, Reserved, Sold) 
                                           VALUES (@FlightId, @CpfBuyer, @Reserved, @Sold);
                                           SELECT CAST(SCOPE_IDENTITY() as int)";

                int saleId = connection.ExecuteScalar<int>(insertSaleQuery, new
                {
                    FlightId = sale.Flight.Id,
                    CpfBuyer = sale.Passengers[0].CPF,
                    Reserved = true,
                    Sold = false
                });

                foreach (var passenger in sale.Passengers)
                {
                    string insertPassengerQuery = @"INSERT INTO PassengerSale (SaleId, CpfPassenger) 
                                                    VALUES (@SaleId, @CpfPassenger)";

                    connection.Execute(insertPassengerQuery, new
                    {
                        SaleId = saleId,
                        CpfPassenger = passenger.CPF
                    });
                }
                return sale;
            }
        }
        public Sale RemoveSale(int Id)
        {
            using (var connection = new SqlConnection(_conn))
            {
                connection.Open();

                var sale = GetSale(Id).Result;

                if (sale == null)
                {
                    Console.WriteLine("Venda não encontrada.");
                    return null;
                }
                if (sale.Reserved == false & sale.Sold == false)
                {
                    Console.WriteLine("Venda cancelada.");
                    return null;
                }
                if (sale.Sold == true)
                {
                    Console.WriteLine("Venda não pode ser cancelada.");
                    return null;
                }
                try //cria um registro na tabela de venda cancelada
                {
                    sale.Reserved = false;
                    sale.Sold = false;

                    string insertCanceledSaleQuery = @"INSERT INTO CanceledSale (Id, FlightId, CpfBuyer, Reserved, Sold) 
                                                    VALUES (@Id, @FlightId, @CpfBuyer, @Reserved, @Sold)";
                    connection.Execute(insertCanceledSaleQuery, new
                    {
                        Id = sale.Id,
                        FlightId = sale.Flight.Id,
                        CpfBuyer = sale.Passengers[0].CPF,
                        Reserved = sale.Reserved,
                        Sold = sale.Sold
                    });

                    string updateSale = "UPDATE Sale SET Reserved = 0, Sold = 0 WHERE Id = @Id";
                    connection.Execute(updateSale, new
                    {
                        Id = sale.Id,
                        FlightId = sale.Flight.Id,
                        CpfBuyer = sale.Passengers[0].CPF,
                        Reserved = sale.Reserved,
                        Sold = sale.Sold
                    });

                    UpdateFlight(sale.Flight.Id, sale.Passengers.Count, "Cancel");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cancelar a venda: " + ex.Message);
                }
                return sale;
            }
        }

        public async Task<bool> SoldSale(int id)
        {
            using (var connection = new SqlConnection(_conn))
            {
                try
                {
                    var sale = GetSale(id).Result;

                    if (sale == null)
                        return false;

                    if (sale.Sold)
                        return false;

                    string query = "UPDATE Sale SET Reserved = 0, Sold = 1 WHERE Id = @Id";

                    connection.Open();
                    var rowsAffected = connection.Execute(query, new { Id = id });
                    connection.Close();

                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task UpdateFlight(int flightId, int count, string operation)
        {
            string baseUri = "https://localhost:7258";
            string requestUri = $"/api/Flights/CheckSeats/{flightId}/{operation}/{count}";
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

    }
}





