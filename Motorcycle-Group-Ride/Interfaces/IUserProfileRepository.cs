using Motorcycle_Group_Ride.Models;

namespace Motorcycle_Group_Ride.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<Profile> CreateUserProfile(Profile profile);
        Task<Profile> GetUserProfile(int userId);
        Task<Profile> UpdateUserProfile(Profile profile);
        Task<bool> DeleteUserProfile(int userId);
        Task<IEnumerable<Profile>> SearchUserProfiles(string query);

    }
}
