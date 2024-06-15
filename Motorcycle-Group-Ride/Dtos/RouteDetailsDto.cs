namespace Motorcycle_Group_Ride.Dtos
{
    public class RouteDetailsDto
    {
        public int RouteId { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public List<string> Steps { get; set; }

        public RouteDetailsDto()
        {
            Steps = new List<string>();
        }
    }
}
