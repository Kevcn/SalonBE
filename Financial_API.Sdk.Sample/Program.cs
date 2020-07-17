using System;
using System.Threading.Tasks;
using Contracts.V1.Requests;
using Refit;

namespace Financial_API.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var Token = string.Empty;

            var identityApi = RestService.For<IIdentityAPI>("https://localhost:5001");
            var financialApi = RestService.For<IFinancialAPI>("http://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(Token)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Test1234!"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdkaccount@gmail.com",
                Password = "Test1234!"
            });

            Token = loginResponse.Content.Token;


            //var allStocks = await financialApi.GetAllAsync();

            var stock = await financialApi.GetAsync(new Guid("8D916C79-1E3E-4F04-9356-147400195335"));
        }
    }
}
