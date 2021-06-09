using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetAuthValidationsQuery : IRequestHandler<GetAuthValidationsRequest, GetAuthValidationsResponse>
    {
        private readonly IAuthValidationService authValidationService;

        public GetAuthValidationsQuery(IAuthValidationService authValidationService)
        {
            this.authValidationService = authValidationService;
        }

        public async Task<GetAuthValidationsResponse> Handle(GetAuthValidationsRequest request,
            CancellationToken cancellationToken)
        {
            bool isAvailable = request.AuthValidationType switch
            {
                AuthValidationType.Username => !await authValidationService.UsernameExists(request.Login),
                AuthValidationType.Email => !await authValidationService.EmailExists(request.Login),
                _ => false
            };

            return new GetAuthValidationsResponse {IsAvailable = isAvailable};
        }
    }
}