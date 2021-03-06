using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    /// <summary>
    ///     Represents service which provides JSON web token bound to each user.
    /// </summary>
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}