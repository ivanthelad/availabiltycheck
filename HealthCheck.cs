using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
namespace Company.Function
{
   public class HealthCheck
    {
        private readonly ILogger _logger;
                private static readonly HttpClient _httpClient = new HttpClient();

    private readonly TelemetryClient _telemetryClient;
        public HealthCheck(ILoggerFactory loggerFactory,TelemetryClient telemetryClient)
        {
            _logger = loggerFactory.CreateLogger<HealthCheck>();
             _telemetryClient = telemetryClient;
        }

        [Function("HealthCheck")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
             TrackAvailability();
        }
                private void TrackAvailability()
        {
            var availability = new AvailabilityTelemetry
            {
                Name = "Endpoint Availability Test",
                RunLocation = "Azure Function",
                Success = false,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                
                var response = _httpClient.GetAsync("https://someappendpoint2344.azurewebsites.net/").Result;
                availability.Success = response.IsSuccessStatusCode;
                availability.Message = response.ReasonPhrase;
            }
            catch (Exception ex)
            {
                availability.Message = ex.Message;
            }
            finally
            {
                _telemetryClient.TrackAvailability(availability);
            }

            _logger.LogInformation($"Service availability: {(availability.Success ? "Available" : "Unavailable")}");
        }
    }
}
