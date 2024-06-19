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
            return _saleRepository.Post(sale);
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
