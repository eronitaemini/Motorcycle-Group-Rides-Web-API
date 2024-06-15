using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Motorcycle_Group_Ride.Models;
using System.IO;

namespace Motorcycle_Group_Ride.Data
{
    public class UserContext: IdentityDbContext<IdentityUser>
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<GroupRide> GroupRides { get; set; }
       
    }
}
//..