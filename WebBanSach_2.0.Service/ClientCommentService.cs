using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Interfaces;
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
        private readonly ICommentRepository _commentRepository;
        private readonly IProductRankRepository _productRankRepository;
        private readonly IMapper _mapper;

        public ClientCommentService(IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository,
                                    ICommentRepository commentRepository,IProductRankRepository productRankRepository, 
                                    IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._applicationUserRepository = applicationUserRepository;
            this._commentRepository = commentRepository;
            this._productRankRepository = productRankRepository;
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
                //update product rating
                var rank = await _productRankRepository.GetSingleByIDAsync(comment.ProductId);
                rank.Rate = listCmt.Average(m => m.Rating);
                await _productRankRepository.UpdateAsync(rank);
            }
            return await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CommentVM>> GetProductListComment(int productId)
        {
            return _mapper.Map<IEnumerable<CommentVM>>(await _commentRepository.GetListCommentByProductId(productId));
        }
    }
}
