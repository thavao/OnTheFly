using Models;
using Repositories;

namespace Services
{
    public class SaleService
    {
        private SaleRepository _saleRepository;

        public SaleService()
        {
            _saleRepository = new();
        }

        public List<Sale> GetSale()
        {
            return _saleRepository.GetSale();
        }

        public Sale GetSale(int id)
        {
            return _saleRepository.GetSale(id);
        }

        public Sale Post(Sale sale)
        {
            return _saleRepository.Post(sale);
        }

    }
}
