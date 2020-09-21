using System.Threading.Tasks;

namespace api_read.service.Interfaces
{
    public interface ICovidApiRequest
    {
        Task<string> Execute(string baseUrl, string serviceUrl, string country);

    }
}
