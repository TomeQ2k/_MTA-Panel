using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class SignInCommand : IRequestHandler<SignInRequest, SignInResponse>
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public SignInCommand(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        public async Task<SignInResponse> Handle(SignInRequest request, CancellationToken cancellationToken)
        {
            var result = await authService.SignIn(request.Login, request.Password)
                         ?? throw new InvalidCredentialsException("Error occurred during signing in");

            return (SignInResponse) new SignInResponse
                    {Token = result.JwtToken, User = mapper.Map<UserAuthDto>(result.User)}
                .LogInformation($"User #{result.User.Id} signed in");
        }
    }
}