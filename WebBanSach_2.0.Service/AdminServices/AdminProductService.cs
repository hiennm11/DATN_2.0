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
    public interface IAdminProductService
    {
        Task<int> AddAuthorToProduct(string productId, int[] authorId);
        Task<int> DeleteAuthorFromProduct(string productId, int authorId);
        Task<IEnumerable<ProductVM>> GetAllProductAsync();
        Task<IndexViewModel<ProductVM>> GetDataAsync(int page, string search, int cateId);
        Task<IEnumerable<CategoryVM>> GetCategoriesListAsync();
        Task<ProductVM> GetDataByIDAsync(string id);
        Task<int> SaveDataAsync(ProductVM viewModel);
        Task<int> DeleteDataAsync(int id);
    }

    public class AdminProductService : IAdminProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorDetailRepository _authorDetailRepository;
        private readonly IMapper _mapper;

        public AdminProductService(IUnitOfWork unitOfWork, IProductRepository productRepository, ICategoryRepository categoryRepository
                                                         , IAuthorDetailRepository authorDetailRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _authorDetailRepository = authorDetailRepository;
            _mapper = mapper;
        }

        public async Task<IndexViewModel<ProductVM>> GetDataAsync(int page, string search, int cateId)
        {
            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetAllAsync(new string[1] { "Category" }));
            var pager = new Pager(_productRepository.GetTotalRow(), page);

            if (!string.IsNullOrEmpty(search))
            {
                data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetBySearchAsync(EntityExtensions.ConvertToUnSign(search)));
                pager = new Pager(data.Count(), page);
            }
            if (cateId > 0)
            {
                data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetByCategoryIdAsync(cateId));
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

        public async Task<ProductVM> GetDataByIDAsync(string id)
        {
            var model = await _productRepository.GetProductByNameIDAsync(id);
            return _mapper.Map<Product, ProductVM>(model);
        }

        public async Task<int> SaveDataAsync(ProductVM viewModel)
        {
            var entity = _mapper.Map<ProductVM, Product>(viewModel);

            if (viewModel.ProductId == 0)
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

        public async Task<int> DeleteDataAsync(int id)
        {
            await _productRepository.ShiftDeleteAsync(id);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<int> AddAuthorToProduct(string productId, int[] authorId)
        {
            var model = await _productRepository.GetProductByNameIDAsync(productId);
            if (authorId != null && authorId.Count() > 0)
            {
                foreach (int item in authorId)
                {
                    var author = await _authorDetailRepository.GetSingleByIDAsync(item);
                    model.Authors.Add(author);
                }
                return await _unitOfWork.SaveAsync();
            }
            return 0;
        }

        public async Task<int> DeleteAuthorFromProduct(string productId, int authorId)
        {
            var model = await _productRepository.GetProductByNameIDAsync(productId);
            model.Authors.Remove(model.Authors.Single(m => m.AuthorId == authorId));
            return await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductVM>> GetAllProductAsync()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(await _productRepository.GetAllAsync());
        }
    }
}
