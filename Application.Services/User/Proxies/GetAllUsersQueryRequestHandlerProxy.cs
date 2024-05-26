using Application.Services.User.Queries;
using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Services.User.Proxies
{    
    public class GetAllUsersQueryRequestHandlerProxy : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private const string Proxied = nameof(GetAllUsersQueryRequestHandler);

        private readonly IUserRepository _userRepository;
        private readonly IRequestTraceService _requestTraceService;
        private readonly ILogger<GetAllUsersQueryRequestHandler> _logger;

        private GetAllUsersQueryRequestHandler _handler;

        public GetAllUsersQueryRequestHandlerProxy(
            IUserRepository userRepository,
            IRequestTraceService requestTraceService,
            ILogger<GetAllUsersQueryRequestHandler> logger)
        {
            _userRepository = userRepository;
            _requestTraceService = requestTraceService;
            _logger = logger;

            _handler = new GetAllUsersQueryRequestHandler(_userRepository);
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request)
        {
            var stopWatch = Stopwatch.StartNew();

            _logger.LogInformation($"Trace Id: {{TraceId}} - Executing {Proxied}", _requestTraceService.TraceId);

            var response = await _handler.Handle(request);

            _logger.LogInformation($"Trace Id: {{TraceId}} - Executed {Proxied}, Time Elapsed {{TimeElapsedInMs}} ms", _requestTraceService.TraceId, stopWatch.ElapsedMilliseconds);

            return response;
        }
    }
}
