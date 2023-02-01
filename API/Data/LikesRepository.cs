using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
            
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
           var users = _context.Users.OrderBy(a => a.UserName).AsQueryable();
           var likes = _context.Likes.AsQueryable();

           if(likesParams.Predicates == "liked")
           {
            likes = likes.Where(a => a.SourceUserId == likesParams.UserId);
            users = likes.Select(a => a.TargetUser);
           }
           if(likesParams.Predicates == "likedBy")
           {
            likes = likes.Where(a => a.TargetUserId == likesParams.UserId);
            users = likes.Select(a => a.SourceUser);
           }

          var likedUsers = users.Select(user => new LikeDto{
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos.FirstOrDefault(a => a.IsMain).Url,
            City = user.City,
            Id = user.Id
           });

           return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(a => a.LikedUsers).FirstOrDefaultAsync(a => a.Id == userId);
        }
    }
}