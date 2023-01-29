using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IIntegrationService
    {
        public string AddNewAccountToIntegration(IntegrationType integratiotype);

        public void HandleCallBack(string code);
    }
}
