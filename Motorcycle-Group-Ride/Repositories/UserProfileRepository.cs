using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Motorcycle_Group_Ride.Data;
using Motorcycle_Group_Ride.Interfaces;
using Motorcycle_Group_Ride.Models;

namespace Motorcycle_Group_Ride.Repositories
{
    public class UserProfileRepository: IUserProfileRepository
    {
        private readonly UserContext _context;

        public UserProfileRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<Profile> CreateUserProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<Profile> GetUserProfile(int userId)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Profile> UpdateUserProfile(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<bool> DeleteUserProfile(int userId)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
                return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Profile>> SearchUserProfiles(string query)
        {
            return await _context.Profiles
                .Where(p => p.Name.Contains(query) || p.Bio.Contains(query))
                .ToListAsync();
        }

    }
}
