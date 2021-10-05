using Datiss.Budget.Entities;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
    public interface IReportService
    {

        Task<Report> GetAsync(int id);

        Task<Report> GetAsync(string name);

    }
}
