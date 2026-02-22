using Application.Services.ReqRes;
using Common.Core.AOP.LogTrace;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using System.Security.Claims;

namespace Application.Services.User.Queries;

[ServiceLocate(typeof(IRequestHandler<GetAllAppNamesRequest, GetAllAppNamesResponse>))]
public class GetAllAppNamesRequestHandler(IServiceProvider serviceProvider, IUserRepository userRepository) : IRequestHandler<GetAllAppNamesRequest, GetAllAppNamesResponse>
{
    // Keep the IServiceProvider which is used in AOP
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IUserRepository _userRepository = userRepository;

    [LogTrace(returnType: typeof(GetAllAppNamesResponse))]
    public async Task<GetAllAppNamesResponse> Handle(GetAllAppNamesRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersWithClaims(cancellationToken).ConfigureAwait(false);
        var roles = users.SelectMany(u => u.Claims.Where(c => c.Name.Equals(ClaimTypes.Role)).Select(c => c.Value)).ToList();

        var appNames = roles.Select(r => r.Split(':')[0]).Distinct().ToList();

        return new GetAllAppNamesResponse(string.Empty, true, appNames);
    }
}
