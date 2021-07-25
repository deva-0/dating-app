using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    ///     Responsible for 'like' functionality
    /// </summary>
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        ///     Adds new like from current user to selected user
        /// </summary>
        /// <param name="username">Username of user to like</param>
        /// <returns>200 OK if successful</returns>
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);

            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("You already liked this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to like user");
        }

        /// <summary>
        ///     Retrieves user likes from database.
        /// </summary>
        /// <param name="predicate">Either 'liked' or 'likedBy'</param>
        /// <returns>Enumerable with liked users or users which liked current user</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
        {
            var users = await _likesRepository.GetUserLikes(predicate, User.GetUserId());
            return Ok(users);
        }
    }
}