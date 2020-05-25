using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.AdminServices
{
    public interface IProductService
    {
        Task<IndexViewModel<ProductVM>> GetDataAsync(int page, string search, int cateId);
        Task<IEnumerable<CategoryVM>> GetCategoriesListAsync();
        Task<ProductVM> GetProductAsync(int id);
        Task<int> SaveProduct(ProductVM product);
        Task<int> DeleteProduct(int id);
    }

    public class AdminProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public AdminProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IndexViewModel<ProductVM>> GetDataAsync(int page, string search, int cateId)
        {
            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetAllAsync(new string[1] { "Category" }));
            var pager = new Pager(_productRepository.GetTotalRow(), page);

            if (search != null && search != "")
            {
                data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetBySearchAsync(search));
                pager = new Pager(data.Count(), page);
            }
            if (cateId > 0)
            {
                data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetByCategoryIntAsync(cateId));
                pager = new Pager(data.Count(), page);
            }

            return new IndexViewModel<ProductVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
        }

        public async Task<IEnumerable<CategoryVM>> GetCategoriesListAsync()
        {
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(await _categoryRepository.GetTrueCategoriesAsync());
        }

        public async Task<ProductVM> GetProductAsync(int id)
        {
            return _mapper.Map<Product, ProductVM>(await _productRepository.GetSingleByIDAsync(id));
        }

        public async Task<int> SaveProduct(ProductVM product)
        {
            var entity = _mapper.Map<ProductVM, Product>(product);
            
            if (product.ProductId == 0)
            {
                entity.CreateBy = "admin"; entity.UpdateBy = "admin";
                entity.Purchase = 0; entity.Star = 0;
                entity.CreateDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;

                await _productRepository.AddAsync(entity);
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                await _productRepository.UpdateAsync(entity);
            }

            return await _unitOfWork.SaveAsync();
        }

        public async Task<int> DeleteProduct(int id)
        {
            await _productRepository.ShiftDeleteAsync(id);
            return await _unitOfWork.SaveAsync();
        }
    }
}
