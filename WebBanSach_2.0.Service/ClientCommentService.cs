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

namespace WebBanSach_2_0.Service
{
    public interface IClientCommentService
    {
        Task<int> AddCommentToDB(CommentVM comment);
        Task<IEnumerable<CommentVM>> GetProductListComment(int productId);
    }

    public class ClientCommentService : IClientCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public ClientCommentService(IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository,
                                    IProductRepository productRepository,ICommentRepository commentRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._applicationUserRepository = applicationUserRepository;
            this._productRepository = productRepository;
            this._commentRepository = commentRepository;
            this._mapper = mapper;
        }

        public async Task<int> AddCommentToDB(CommentVM comment)
        {
            var user = _applicationUserRepository.GetUserByUserName(comment.UserId);
            comment.UserId = user.Id;
            var cmt = _mapper.Map<Comment>(comment);
            await _commentRepository.AddAsync(cmt);

            if(comment.Rating > 0)
            {
                var listCmt = await _commentRepository.GetListCommentByProductId(comment.ProductId);
                var product = await _productRepository.GetSingleByIDAsync(comment.ProductId);
                product.Star = listCmt.Average(m => m.Rating);
                await _productRepository.UpdateAsync(product);
            }
            return await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CommentVM>> GetProductListComment(int productId)
        {
            return _mapper.Map<IEnumerable<CommentVM>>(await _commentRepository.GetListCommentByProductId(productId));
        }
    }
}
