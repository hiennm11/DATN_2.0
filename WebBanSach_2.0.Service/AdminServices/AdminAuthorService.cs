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
    class AdminAuthorService : IAdminAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorDetailRepository _authorDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AdminAuthorService(IUnitOfWork unitOfWork, IAuthorDetailRepository authorDetailRepository, IProductRepository productRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._authorDetailRepository = authorDetailRepository;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        public async Task<int> AddProductToAuthor(int authorId, int[] productId)
        {
            var model = await _authorDetailRepository.GetAuthorById(authorId);
            if(productId != null && productId.Count() > 0)
            {
                foreach(var item in productId)
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
            await _authorDetailRepository.ShiftDeleteAsync(id);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<int> DeleteProductFromAuthor(int authorId, int productId)
        {
            var model = await _authorDetailRepository.GetAuthorById(authorId);
            model.Products.Remove(model.Products.Single(p => p.ProductId == productId));
            return await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<AuthorVM>> GetAllAuthorAsync()
        {
            return _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorVM>>(await _authorDetailRepository.GetAllAsync());
        }

        public async Task<IndexViewModel<AuthorVM>> GetDataAsync(int page, string search)
        {
            var data = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorVM>>(await _authorDetailRepository.GetAllAsync());
            var pager = new Pager(_authorDetailRepository.GetTotalRow(), page);

            if (!string.IsNullOrEmpty(search))
            {
                data = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorVM>>(await _authorDetailRepository.GetBySearchAsync(search));
                pager = new Pager(data.Count(), page);
            }

            return new IndexViewModel<AuthorVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
        }

        public async Task<AuthorVM> GetDataByIDAsync(int id)
        {
            return _mapper.Map<Author, AuthorVM>(await _authorDetailRepository.GetAuthorById(id));
        }

        public async Task<int> SaveDataAsync(AuthorVM viewModel)
        {
            var entity = _mapper.Map<AuthorVM, Author>(viewModel);
            entity.NameAlias = EntityExtensions.ConvertToUnSign(viewModel.Name);

            if (viewModel.AuthorId == 0)
            {
                entity.CreateBy = "admin"; entity.UpdateBy = "admin";
                entity.CreateDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;
                entity.UniqueStringKey = Guid.NewGuid();

                await _authorDetailRepository.AddAsync(entity);
            }
            else
            {
                entity.UpdatedDate = DateTime.Now;
                await _authorDetailRepository.UpdateAsync(entity);
            }

            return await _unitOfWork.SaveAsync();
        }
    }
}
