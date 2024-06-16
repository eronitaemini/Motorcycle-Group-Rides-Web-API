using Motorcycle_Group_Ride.Models;

namespace Motorcycle_Group_Ride.Interfaces
{
    public interface IRouteRepository
    {
        Task<GroupRide> CreateRoute(GroupRide route);
        Task<GroupRide> GetRouteDetails(int routeId);
        Task<GroupRide> UpdateRoute(GroupRide route);
        Task<bool> DeleteRoute(int routeId);
        Task<IEnumerable<GroupRide>> GetUserRoutes(int userId);
    }
}
