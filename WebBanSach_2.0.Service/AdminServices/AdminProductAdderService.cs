using AutoMapper;
using System;
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

namespace WebBanSach_2_0.Service.AdminServices
{
    public class AdminProductAdderService : IAdminProductAdderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductAdderRepository _productAdderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AdminProductAdderService(IUnitOfWork unitOfWork, IProductAdderRepository productAdderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productAdderRepository = productAdderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IndexViewModel<ProductAdderVM>> GetDataAsync(int page, int pageSize, int? categoryId, string search = null)
        {
            var data = _mapper.Map<IEnumerable<ProductAdderVM>>(await _productAdderRepository.GetAllAsync());
            var pager = new Pager(_productAdderRepository.GetTotalRow(), page);

            if (!string.IsNullOrEmpty(search))
            {
                data = _mapper.Map<IEnumerable<ProductAdderVM>>(await _productAdderRepository.GetBySearchAsync(EntityExtensions.ConvertToUnSign(search)));
                pager = new Pager(data.Count(), page);
            }
            if (categoryId.HasValue && categoryId > 0)
            {
                data = _mapper.Map<IEnumerable<ProductAdderVM>>(await _productAdderRepository.GetByCategoryAsync(categoryId.Value));
                pager = new Pager(data.Count(), page);
            }

            return new IndexViewModel<ProductAdderVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
        }

        public async Task<ProductAdderVM> GetDataByIDAsync(int id)
        {
            return _mapper.Map<ProductAdderVM>(await _productAdderRepository.GetSingleByIDAsync(id));
        }

        public async Task<int> SaveDataAsync(ProductAdderVM viewModel)
        {
            var adder = _mapper.Map<ProductAdder>(viewModel);
            adder.PurchaseDate = DateTime.Now;
            var product = await _productRepository.GetProductByAdderAsync(adder, EntityExtensions.ConvertToUnSign(adder.Name));
            if(product != null)
            {
                product.AvailableQuantity += adder.AvailableQuantity;

                await _productAdderRepository.AddAsync(adder);
                await _productRepository.UpdateAsync(product);
                return await _unitOfWork.SaveAsync();
            }
            return 0;
        }
    }
}
