using Contracts.V1.Requests;
using Contracts.V1.Responses;
using Refit;
using System.Threading.Tasks;

namespace Financial_API.Sdk
{
    public interface IIdentityAPI
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest registrationRequest);
        
        [Post("/api/v1/identity/login")]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest loginRequest);
    }
}
