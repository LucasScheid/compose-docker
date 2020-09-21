using api_read.service.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace api_read.service.Services
{
    public class CovidApiRequest : ICovidApiRequest
    {
        public async Task<string> Execute(string baseUrl, string serviceUrl, string country)
        {

            try
            {
                var finalUrl = string.Concat(baseUrl, serviceUrl, country);

                using HttpClient httpClient = new HttpClient();

                var result = await httpClient.GetAsync(finalUrl);

                if (result.IsSuccessStatusCode)
                    return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"GET: {string.Concat(baseUrl, serviceUrl, country)} - Error: {ex.Message}");
            }

            return string.Empty;

        }
    }
}
