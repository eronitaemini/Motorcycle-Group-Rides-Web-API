using Microsoft.EntityFrameworkCore;
using Motorcycle_Group_Ride.Data;
using Motorcycle_Group_Ride.Interfaces;
using Motorcycle_Group_Ride.Models;

namespace Motorcycle_Group_Ride.Repositories
{
    public class RouteRepository: IRouteRepository
    {
        private readonly UserContext _context;

        public RouteRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<GroupRide> CreateRoute(GroupRide route)
        {
            _context.GroupRides.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<GroupRide> GetRouteDetails(int routeId)
        {
            return await _context.GroupRides.FirstOrDefaultAsync(r => r.Id == routeId);
        }

        public async Task<GroupRide> UpdateRoute(GroupRide route)
        {
            _context.GroupRides.Update(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<bool> DeleteRoute(int routeId)
        {
            var route = await _context.GroupRides.FirstOrDefaultAsync(r => r.Id == routeId);
            if (route == null)
                return false;

            _context.GroupRides.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GroupRide>> GetUserRoutes(int userId)
        {
            return await _context.GroupRides.Where(r => r.UserId == userId).ToListAsync();
        }
    }
}
//.