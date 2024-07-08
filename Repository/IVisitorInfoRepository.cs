using Webapi.Models;
using System.Threading.Tasks;

namespace Webapi.Repository
{
    public interface IVisitorInfoRepository
    {
        Task<VisitorInfo> GetVisitorInfoAsync(string visitorName, string clientIp);
    }
}
