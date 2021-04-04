using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models.Database;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Handlers
{
    public class LoginHandler : IRequestHandler<LoginRequest, ResponseJson>
    {
        private readonly SignInManager<User> _signInManager;

        public LoginHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ResponseJson> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            ResponseJson responseJson;
            SignInResult result = await _signInManager.PasswordSignInAsync(request.LoginRequestJson.UserName, request.LoginRequestJson.Password, false, false);
            if (result.Succeeded)
            {
                responseJson = new ResponseJson
                {
                    Code = 200,
                    Description = new Description
                    {
                        Error = 0,
                        Content = "Authentication completed successfully!"
                    }
                };
            }
            else
            {
                responseJson = new ResponseJson
                {
                    Code = 401,
                    Description = new Description
                    {
                        Error = 1,
                        Details = new List<Details>
                        {
                            new Details
                            {
                                Name = "Unauthorized",
                                Description = "Incorrect username or password!"
                            }
                        },
                        Content = "Authentication failed!"
                    }
                };
            }
            return responseJson;
        }
    }
}
