using Dapper;
using Microsoft.Data.SqlClient;
using Models;

namespace Repositories
{
    public class SaleRepository
    {

        public SaleRepository() { }

        public List<Sale> GetSale()
        {
            throw new NotImplementedException();
        }

        public Sale GetSale(int id)
        {
            throw new NotImplementedException();
        }

        public Sale Post(Sale sale)
        {
            string strConn = "Data Source=127.0.0.1;Initial Catalog=DBSales;User Id=sa;Password=SqlServer2019!;TrustServerCertificate=Yes;";

            using (var connection = new SqlConnection(strConn))
            {
                connection.Open();

                string insertSaleQuery = @"INSERT INTO Sale (FlightId, CpfBuyer, Reserved, Sold) 
                                           VALUES (@FlightId, @CpfBuyer, @Reserved, @Sold);
                                           SELECT CAST(SCOPE_IDENTITY() as int)";

                int saleId = connection.ExecuteScalar<int>(insertSaleQuery, new
                {
                    FlightId = sale.Flight.Id,
                    CpfBuyer = sale.Passengers[0].CPF,
                    Reserved = sale.Reserved,
                    Sold = sale.Sold
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
    }
}
