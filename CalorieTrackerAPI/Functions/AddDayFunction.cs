using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Services;

namespace CalorieTrackerAPI.Functions
{
    public class AddDayFunction
    {
        private readonly CalorieTrackerService _calorieTrackerService;
        private readonly DataService _dataService;

        public AddDayFunction(CalorieTrackerService calorieTrackerService, DataService dataService)
        {
            _calorieTrackerService = calorieTrackerService ?? throw new ArgumentNullException(nameof(calorieTrackerService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [Function("AddDay")]
        public async Task<HttpResponseData> AddDay(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "days")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("AddDay");
            logger.LogInformation("Processing AddDay request.");

            // Read the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var day = JsonConvert.DeserializeObject<Day>(requestBody);

            // Validate the input data
            if (day == null || day.Foods == null || day.Foods.Count == 0)
            {
                logger.LogWarning("Invalid input data received.");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid input data. Ensure the day object and its foods are provided.");
                return badResponse;
            }

            // Calculate total calories for the day
            int totalCalories = _calorieTrackerService.CalculateTotalCalories(day.Foods);

            // Append the day data to the local file
            _dataService.AppendDayToFile(day);

            // Return success response
            var response = req.CreateResponse(HttpStatusCode.Created); // Use 201 Created status code
            await response.WriteAsJsonAsync(new { message = "Day added successfully", totalCalories });
            return response;
        }
    }
}
