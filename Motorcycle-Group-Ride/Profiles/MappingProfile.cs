using Motorcycle_Group_Ride.Dtos;
using Motorcycle_Group_Ride.Models;
using AutoMapper;
namespace Motorcycle_Group_Ride.Profiles
{
    public class MappingProfile: AutoMapper.Profile
    {


        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AutoMapper.Profile, ProfileDto>().ReverseMap();
            CreateMap<GroupRide, GroupRideDto>().ReverseMap();
            CreateMap<GroupRide, RouteDetailsDto>().ReverseMap();
            CreateMap<GroupRide, RouteUpdateDto>().ReverseMap();
        }

    }
}
