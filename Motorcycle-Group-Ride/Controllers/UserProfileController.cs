using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Motorcycle_Group_Ride.Data;
using Motorcycle_Group_Ride.Dtos;
using Motorcycle_Group_Ride.Models;
using static Motorcycle_Group_Ride.Dtos.UserPRofileCreateDto;

namespace Motorcycle_Group_Ride.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController: ControllerBase
    {
        private readonly UserContext _context;

        public UserProfileController(UserContext context)
        {
            _context = context;
        }

        // POST api/userprofile
        [HttpPost]
        public async Task<ActionResult<ProfileDto>> CreateUserProfile(UserProfileCreateDto userProfileDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userProfileDto.UserId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var profile = new Profile
                {
                    UserId = userProfileDto.UserId,
                    Name = userProfileDto.Name,
                    Bio = userProfileDto.Bio
                };

                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();

                var profileDto = new ProfileDto
                {
                    Id = profile.Id,
                    UserId = profile.UserId,
                    Name = profile.Name,
                    Bio = profile.Bio
                };

                return CreatedAtAction(nameof(GetUserProfile), new { userId = profileDto.UserId }, profileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/userprofile/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileDto>> GetUserProfile(int userId)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                return NotFound("User profile not found");
            }

            var profileDto = new ProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Name = profile.Name,
                Bio = profile.Bio
            };

            return Ok(profileDto);
        }

        // PUT api/userprofile/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, UserProfileUpdateDto userProfileDto)
        {
            try
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

                if (profile == null)
                {
                    return NotFound("User profile not found");
                }

                profile.Name = userProfileDto.Name;
                profile.Bio = userProfileDto.Bio;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/userprofile/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserProfile(int userId)
        {
            try
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

                if (profile == null)
                {
                    return NotFound("User profile not found");
                }

                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/userprofile/search/{query}
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> SearchUserProfiles(string query)
        {
            var profiles = await _context.Profiles
                .Where(p => p.Name.Contains(query) || p.Bio.Contains(query))
                .ToListAsync();

            if (profiles == null || profiles.Count == 0)
            {
                return NotFound("No user profiles found matching the query");
            }

            var profileDtos = profiles.Select(p => new ProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Name = p.Name,
                Bio = p.Bio
            });

            return Ok(profileDtos);
        }
    }

}

//..