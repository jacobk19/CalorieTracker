using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Services;

namespace CalorieTrackerAPI.Functions
{
    public class GetTotalCaloriesForDayFunction
    {
        private readonly DataService _dataService;
        private readonly CalorieTrackerService _calorieTrackerService;

        public GetTotalCaloriesForDayFunction(DataService dataService, CalorieTrackerService calorieTrackerService)
        {
            _dataService = dataService;
            _calorieTrackerService = calorieTrackerService;
        }

        [Function("GetTotalCaloriesForDay")]
        public async Task<HttpResponseData> GetTotalCaloriesForDay(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "days/{dayId}")] HttpRequestData req,
            string dayId,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetTotalCaloriesForDay");
            logger.LogInformation($"Calculating total calories for day ID: {dayId}");

            // Read all days from the file
            var days = _dataService.ReadDaysFromFile(); 
            
            // Convert dayId to integer
            if (!int.TryParse(dayId, out int dayIdInt))
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid day ID format.");
                return badRequestResponse;
            }

            // Find the day using the converted integer ID
            var day = days.FirstOrDefault(d => d.Id == dayIdInt); // Assuming Day has an Id property of type int

            if (day == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Day not found.");
                return notFoundResponse;
            }

            // Calculate total calories for the day
            int totalCalories = _calorieTrackerService.CalculateTotalCalories(day.Foods);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { totalCalories });
            return response;
        }
    }
}
