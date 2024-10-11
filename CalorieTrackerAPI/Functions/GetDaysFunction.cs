using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CalorieTrackerAPI.Services; // Assuming DataService is in Services namespace
using CalorieTrackerAPI.Models;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace CalorieTrackerAPI.Functions
{
    public class GetDaysFunction
    {
        private readonly DataService _dataService;

        public GetDaysFunction(DataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService)); // Null check for dataService
        }

        [Function("GetDays")]
        public async Task<HttpResponseData> GetDays(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "days")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("GetDays");
            logger.LogInformation("Processing GetDays request.");

            try
            {
                List<Day> days = _dataService.ReadDaysFromFile(); // Assume it's a synchronous call

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(days);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while getting days: {ex.Message}");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred while processing your request.");
                return errorResponse;
            }
        }
    }
}