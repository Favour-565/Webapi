using Microsoft.EntityFrameworkCore;
using Webapi.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace Webapi.Repository
{
    public class VisitorInfoRepository : IVisitorInfoRepository
    {
        private readonly ApiDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _default;

        public VisitorInfoRepository(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _httpClient = new HttpClient();
            _default = configuration["Default"]; // Fetch the weather API key from configuration
        }

        public async Task<VisitorInfo> GetVisitorInfoAsync(string visitorName, string clientIp)
        {
            try
            {
                var location = await GetLocationFromIp(clientIp);
                var temperature = await GetTemperature(location);

                var visitorInfo = new VisitorInfo
                {
                    VisitorName = visitorName,
                    ClientIp = clientIp,
                    Location = location,
                    Temperature = temperature,
                    VisitTime = DateTime.UtcNow
                };

                _context.VisitorInfos.Add(visitorInfo);
                await _context.SaveChangesAsync();

                return visitorInfo;
            }
            catch (Exception ex)
            {
                // Handle and log exception (e.g., log to file, console, etc.)
                throw new ApplicationException("An error occurred while retrieving visitor info.", ex);
            }
        }

        private async Task<string> GetLocationFromIp(string ip)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
                var json = JObject.Parse(response);
                return json["city"]?.ToString() ?? "Unknown";
            }
            catch (Exception ex)
            {
                // Handle and log exception
                return "Unknown";
            }
        }

        private async Task<float> GetTemperature(string location)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"http://api.openweathermap.org/data/2.5/weather?q={location}&units=metric&appid={_default}");
                var json = JObject.Parse(response);
                return json["main"]?["temp"]?.Value<float>() ?? 0.0f;
            }
            catch (Exception ex)
            {
                // Handle and log exception
                return 0.0f;
            }
        }
    }
}
