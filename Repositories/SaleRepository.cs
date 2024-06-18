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

            _passengerUri = "https://localhost:7034";
            _flightUri = "https://localhost:7034";
        }

        public async Task<List<Sale>> GetSale()
        {
            var list = new List<Sale>();
            using var connection = new SqlConnection(_conn);
            connection.Open();

            var t1 = ApiConsume<List<Passenger>>.Get(_passengerUri, "/GetPassengers");
            var t2 = ApiConsume<List<Flight>>.Get(_flightUri, "/GetFlights");
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

            var t1 = ApiConsume<List<Passenger>>.Get(_passengerUri, "/GetPassengers");
            var t2 = ApiConsume<Flight>.Get(_flightUri, $"/GetFlights/{row.FlightId}");
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
            throw new NotImplementedException();
        }

    }
}
