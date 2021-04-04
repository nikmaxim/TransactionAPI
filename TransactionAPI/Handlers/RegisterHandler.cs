using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models.Database;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, ResponseJson>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<ResponseJson> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            ResponseJson responseJson;
            User user = new User
            {
                Email = request.RegisterRequestJson.Email,
                UserName = request.RegisterRequestJson.UserName
            };
            IdentityResult result = await _userManager.CreateAsync(user, request.RegisterRequestJson.Password);    //Registering a new user            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);                                          //Authenticating this user
                responseJson = new ResponseJson
                {
                    Code = 200,
                    Description = new Description
                    {
                        Error = 0,
                        Content = "Registration completed successfully!"
                    }
                };
            }
            else //if the result includes errors
            {
                responseJson = new ResponseJson
                {
                    Code = 422,
                    Description = new Description
                    {
                        Error = result.Errors.Count(),
                        Details = new List<Details>(),
                        Content = "Registration failed!"
                    }
                };
                foreach (IdentityError error in result.Errors)
                {
                    responseJson.Description.Details.Add(new Details
                    {
                        Name = error.Code,
                        Description = error.Description
                    });
                }
            }
            return responseJson;
        }
    }
}
