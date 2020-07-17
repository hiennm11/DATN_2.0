using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories.Interfaces;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Service.Interfaces;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.AdminServices
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AdminCategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IProductRepository productRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._categoryRepository = categoryRepository;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        public async Task<int> DeleteDataAsync(int id)
        {
            var list = await _productRepository.GetByCategoryIdAsync(id);
            if(list != null)
            {
                foreach(var item in list)
                {
                    item.CategoryId = 1;
                    await _productRepository.UpdateAsync(item);
                }
            }
            await _categoryRepository.ShiftDeleteAsync(id);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<CategoryVM> GetDataByIDAsync(int id)
        {
            return _mapper.Map<Category, CategoryVM>(await _categoryRepository.GetSingleByIDAsync(id));
        }

        public async Task<IndexViewModel<CategoryVM>> GetDataAsync(int page, string search)
        {
            var data = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(await _categoryRepository.GetAllAsync());
            var pager = new Pager(_categoryRepository.GetTotalRow(), page);

            if (!string.IsNullOrEmpty(search))
            {
                data = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(await _categoryRepository.GetBySearchAsync(search));
                pager = new Pager(data.Count(), page);
            }            

            return new IndexViewModel<CategoryVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
        }

        public async Task<int> SaveDataAsync(CategoryVM viewModel)
        {
            var entity = _mapper.Map<CategoryVM, Category>(viewModel);
            entity.NameAlias = EntityExtensions.ConvertToUnSign(viewModel.CategoryName);

            if (viewModel.CategoryId == 0)
            {
                entity.CreateBy = "admin"; entity.UpdateBy = "admin";
                entity.CreateDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;
                entity.UniqueStringKey = Guid.NewGuid();

                await _categoryRepository.AddAsync(entity);
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                await _categoryRepository.UpdateAsync(entity);
            }

            return await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryVM>> GetCategoriesListAsync()
        {
            return _mapper.Map<IEnumerable<CategoryVM>>(await _categoryRepository.GetAllAsync());
        }
    }
}
