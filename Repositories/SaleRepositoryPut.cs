using Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Repositories
{
    public class SaleRepositoryPut
    {

        private readonly string conn = "Data Source=127.0.0.1; Initial Catalog=DBSales; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
        public SaleRepositoryPut() { }

        public List<Sale> GetSale()
        {
            throw new NotImplementedException();
        }

        public Sale GetSale(int id)
        {
            throw new NotImplementedException();
        }

        public Sale PutSale(Sale sale)
        {
            using (var db = new SqlConnection(conn))
            {
                string query = "UPDATE Sale SET " +
                    "[FlightId] = @Flight," +
                    "[CpfBuyer] =  @CpfBuyer," +
                    "[Reserved] = @Reserved," +
                    "[Sold] = @Sold " +
                    "WHERE Id = @Id;";
                db.Open();
                db.Execute(query, new
                {
                    Flight = sale.Flight.Id,
                    CpfBuyer = sale.Passengers[0].CPF,
                    Reserved = sale.Reserved,
                    Sold = sale.Sold,
                    Id = sale.Id,
                });
                db.Close();
            }
            return sale;
        }

        public Sale Post(Sale sale)
        {
            throw new NotImplementedException();
        }

    }
}
