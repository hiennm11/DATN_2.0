using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Service.Interfaces;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service
{
    public class ClientProductService : IClientProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public ClientProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, 
                                    IDiscountRepository discountRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._productRepository = productRepository;
            this._discountRepository = discountRepository;
            this._mapper = mapper;
        }

        public async Task<IndexViewModel<ProductVM>> GetAllProducts(string categoryId = null, string search = null, int pageSize = 16, int page = 1)
        {
            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetPagingAsync(page, pageSize, new string[2] { "Category", "Discount" }));
            var pager = new Pager(_productRepository.GetTotalRow(), page, pageSize);

            if (!string.IsNullOrEmpty(search))
            {
                categoryId = string.Empty;
                data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetBySearchPagingAsync(EntityExtensions.ConvertToUnSign(search), page, pageSize));
                pager = new Pager(data.Count(), page);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                search = string.Empty;
                data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetByCategoryPagingAsync(categoryId, page, pageSize));
                pager = new Pager(data.Count(), page);
            }

            return new IndexViewModel<ProductVM>()
            {
                Items = data.Where(m => m.Status == true),
                Pager = pager
            };
        }

        public async Task<IEnumerable<ProductVM>> GetHotProducts()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetHotProductAsync());
        }

        public async Task<IEnumerable<ProductVM>> GetNewProducts()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetNewProductAsync());
        }

        public async Task<ProductVM> GetProductByNameAlias(string id)
        {
            return _mapper.Map<Product, ProductVM>(await _productRepository.GetProductByNameIDAsync(id));
        }

        public async Task<IEnumerable<ProductVM>> GetProductsByCategoryId(int id)
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetByCategoryIdAsync(id));
        }

        public async Task<IEnumerable<DiscountVM>> GetProductsDiscounts()
        {
            return _mapper.Map<IEnumerable<DiscountVM>>(await _discountRepository.GetProductDiscount());
        }
    }
}
