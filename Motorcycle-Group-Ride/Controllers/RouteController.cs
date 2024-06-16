using Microsoft.AspNetCore.Mvc;
using Motorcycle_Group_Ride.Dtos;
using Newtonsoft.Json;

namespace Motorcycle_Group_Ride.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController: ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _googleMapsApiKey = "YOUR_GOOGLE_MAPS_API_KEY";

        public RouteController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET api/route/calculate?startLocationId={startLocationId}&endLocationId={endLocationId}
        [HttpGet("calculate")]
        public async Task<ActionResult<RouteDetailsDto>> CalculateRoute(int startLocationId, int endLocationId)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var requestUri = $"https://maps.googleapis.com/maps/api/directions/json?" +
                                 $"origin={startLocationId}&destination={endLocationId}&key={_googleMapsApiKey}";

                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var routeDetails = ParseRouteDetails(json);
                    return Ok(routeDetails);
                }
                else
                {
                    return BadRequest("Failed to calculate route");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/route/{routeId}
        [HttpGet("{routeId}")]
        public async Task<ActionResult<RouteDetailsDto>> GetRouteDetails(int routeId)
        {
            try
            {
                // Mock implementation: Replace with actual route details retrieval logic (e.g., database query)
                var routeDetails = new RouteDetailsDto
                {
                    RouteId = routeId,
                    StartLocationId = 1,
                    EndLocationId = 2,
                    Distance = "10 km",
                    Duration = "15 mins",
                    Steps = new List<string> { "Step 1", "Step 2", "Step 3" }
                };

                return Ok(routeDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/route/{routeId}
        [HttpPut("{routeId}")]
        public async Task<IActionResult> UpdateRoute(int routeId, RouteUpdateDto routeUpdateDto)
        {
            try
            {
                // Mock implementation: Replace with actual update logic (e.g., database update)
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/route/{routeId}
        [HttpDelete("{routeId}")]
        public async Task<IActionResult> DeleteRoute(int routeId)
        {
            try
            {
                // Mock implementation: Replace with actual delete logic (e.g., database delete)
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/route/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<RouteDetailsDto>>> GetUserRoutes(int userId)
        {
            try
            {
                // Mock implementation: Replace with actual user-specific routes retrieval logic (e.g., database query)
                var userRoutes = new List<RouteDetailsDto>
                {
                    new RouteDetailsDto
                    {
                        RouteId = 1,
                        StartLocationId = 1,
                        EndLocationId = 2,
                        Distance = "10 km",
                        Duration = "15 mins",
                        Steps = new List<string> { "Step 1", "Step 2", "Step 3" }
                    },
                    new RouteDetailsDto
                    {
                        RouteId = 2,
                        StartLocationId = 2,
                        EndLocationId = 3,
                        Distance = "15 km",
                        Duration = "20 mins",
                        Steps = new List<string> { "Step 1", "Step 2", "Step 3" }
                    }
                };

                return Ok(userRoutes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private RouteDetailsDto ParseRouteDetails(string json)
        {
            var routeDetails = new RouteDetailsDto();

            dynamic data = JsonConvert.DeserializeObject(json);
            if (data != null && data.routes.Count > 0)
            {
                var route = data.routes[0];
                routeDetails.Distance = route.legs[0].distance.text;
                routeDetails.Duration = route.legs[0].duration.text;

                // Extract steps if needed
                foreach (var step in route.legs[0].steps)
                {
                    routeDetails.Steps.Add(step.html_instructions);
                }
            }

            return routeDetails;
        }

    }
}
//..