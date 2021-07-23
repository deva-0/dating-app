using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    /// <summary>
    /// Creates abstraction over database abstraction (DbContext), provides customized methods  
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new UserRepository with given database session.
        /// </summary>
        /// <param name="context">DI Database session</param>
        /// <param name="mapper">DI AutoMapper interface</param>
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        /// <summary>
        /// Retrieves member with passed username from database.
        /// </summary>
        /// <param name="username">Username of member to retrieve</param>
        /// <returns>Data transfer object of a member</returns>
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates new PagedList with retrieved members.
        /// </summary>
        /// <param name="userParams">Object with custom parameters from request query string</param>
        /// <returns>New PagedList with filtered amount of members</returns>
        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        /// <summary>
        /// Retrieves user with specified id from database.
        /// </summary>
        /// <param name="id">Id of user to retrieve</param>
        /// <returns>User object</returns>
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Retrieves user with passed username from database.
        /// </summary>
        /// <param name="username">Username of user to retrieve</param>
        /// <returns>User object</returns>
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        /// <summary>
        /// Retrieves all users with from database.
        /// </summary>
        /// <returns>List of all users</returns>
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        /// <summary>
        /// Saves all changes to database asynchronously
        /// </summary>
        /// <returns>True if there were changes, False if nothing changed</returns>
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        /// <summary>
        /// Updates information about user in database.
        /// </summary>
        /// <param name="user">User object to update</param>
        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}