using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models.Database;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Handlers
{
    public class LogoutHandler : IRequestHandler<LogoutRequest, ResponseJson>
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ResponseJson> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return new ResponseJson
            {
                Code = 200,
                Description = new Description
                {
                    Error = 0,
                    Content = "Logout was completed successfully!"
                }
            };
        }
    }
}
