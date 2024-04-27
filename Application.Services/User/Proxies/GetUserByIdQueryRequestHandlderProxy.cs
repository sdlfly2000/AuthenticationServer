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
    [ServiceLocate(typeof(IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>))]
    public class GetUserByIdQueryRequestHandlderProxy : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private const string Proxied = nameof(GetUserByIdQueryRequestHandlder);

        private readonly IUserRepository _userRepository;
        private readonly IRequestTraceService _requestTraceService;
        private readonly ILogger<GetUserByIdQueryRequestHandlder> _logger;

        private GetUserByIdQueryRequestHandlder _handler;

        public GetUserByIdQueryRequestHandlderProxy(
            IUserRepository userRepository,
            IRequestTraceService requestTraceService,
            ILogger<GetUserByIdQueryRequestHandlder> logger)
        {
            _userRepository = userRepository;
            _requestTraceService = requestTraceService;
            _logger = logger;

            _handler = new GetUserByIdQueryRequestHandlder(_userRepository);
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request)
        {
            var stopWatch = Stopwatch.StartNew();

            _logger.LogInformation($"Trace Id: {{TraceId}} - Executing {Proxied}", _requestTraceService.TraceId);

            var response = await _handler.Handle(request);

            _logger.LogInformation($"Trace Id: {{TraceId}} - Executed {Proxied}, Time Elapsed {{TimeElapsedInMs}} ms", _requestTraceService.TraceId, stopWatch.ElapsedMilliseconds);


            return response;
        }
    }
}
