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
    public interface IAdminCategoryService
    {
        Task<IndexViewModel<CategoryVM>> GetDataAsync(int page, string search);
        Task<CategoryVM> GetCategoryAsync(int id);
        Task<int> SaveCategory(CategoryVM Category);
        Task<int> DeleteCategory(int id);
    }
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public AdminCategoryService(IUnitOfWork unitOfWork, ICategoryRepository CategoryRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        public Task<int> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryVM> GetCategoryAsync(int id)
        {
            return _mapper.Map<Category, CategoryVM>(await _categoryRepository.GetSingleByIDAsync(id));
        }

        public async Task<IndexViewModel<CategoryVM>> GetDataAsync(int page, string search)
        {
            var data = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(await _categoryRepository.GetAllAsync());
            var pager = new Pager(_categoryRepository.GetTotalRow(), page);

            if (search != null && search != "")
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

        public async Task<int> SaveCategory(CategoryVM Category)
        {
            var entity = _mapper.Map<CategoryVM, Category>(Category);

            if (Category.CategoryId == 0)
            {
                entity.CreateBy = "admin"; entity.UpdateBy = "admin";
                entity.CreateDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;

                await _categoryRepository.AddAsync(entity);
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                await _categoryRepository.UpdateAsync(entity);
            }

            return await _unitOfWork.SaveAsync();
        }
    }
}
