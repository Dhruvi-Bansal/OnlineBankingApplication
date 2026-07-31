using System.Threading.Tasks;

namespace OnlineBankingApplication.Services
{

    public interface IAIService
    {
        Task<string> AskAI(string applicationContext,string userQuestion);
    }
}