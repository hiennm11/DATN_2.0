using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.AdminServices
{
    public interface IAdminDiscountService
    {
        Task<int> AddProductToDiscount(int discountId, int[] productId);
        Task<int> DeleteProductFromDiscount(int discountId, int productId);
        Task<IndexViewModel<DiscountVM>> GetDataAsync(string search, int pageSize, int page);
        Task<DiscountVM> GetDataByIDAsync(int id);
        Task<int> SaveDataAsync(DiscountVM viewModel);
        Task<int> DeleteDataAsync(int id);
    }

    public class AdminDiscountService : IAdminDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AdminDiscountService(IUnitOfWork unitOfWork, IDiscountRepository discountRepository, IProductRepository productRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._discountRepository = discountRepository;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        public async Task<int> AddProductToDiscount(int discountId, int[] productId)
        {
            var model = await _discountRepository.GetDiscountById(discountId, new string[1] { "Products" });
            if (productId != null && productId.Count() > 0)
            {
                foreach (var item in productId)
                {
                    var product = await _productRepository.GetSingleByIDAsync(item);
                    model.Products.Add(product);
                }
                return await _unitOfWork.SaveAsync();
            }
            return 0;
        }

        public async Task<int> DeleteDataAsync(int id)
        {
            await _discountRepository.ShiftDeleteAsync(id);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<int> DeleteProductFromDiscount(int discountId, int productId)
        {
            var model = await _discountRepository.GetDiscountById(discountId, new string[1] { "Products" });
            model.Products.Remove(model.Products.Single(p => p.ProductId == productId));
            return await _unitOfWork.SaveAsync();
        }

        public async Task<IndexViewModel<DiscountVM>> GetDataAsync(string search, int pageSize, int page)
        {
            var totalRow = _discountRepository.GetTotalRow();
            var pager = new Pager(totalRow, page, pageSize);

            var result = new IndexViewModel<DiscountVM>
            {
                Items = _mapper.Map<IEnumerable<DiscountVM>>(await _discountRepository.GetPagingAsync(page, pageSize)),
                Pager = pager
            };

            return result;
        }

        public async Task<DiscountVM> GetDataByIDAsync(int id)
        {
            return _mapper.Map<DiscountVM>(await _discountRepository.GetDiscountById(id, new string[1] { "Products" }));
        }

        public async Task<int> SaveDataAsync(DiscountVM viewModel)
        {
            var entity = _mapper.Map<Discount>(viewModel);
            if (!string.IsNullOrEmpty(entity.DiscountCode))
            {
                entity.DiscountCode = entity.DiscountCode.ToUpper();
            }
            if (viewModel.DiscountId == 0)
            {
               await  _discountRepository.AddAsync(entity);
            }
            else
            {
                await _discountRepository.UpdateAsync(entity);
            }

            return await _unitOfWork.SaveAsync();
        }
    }
}
