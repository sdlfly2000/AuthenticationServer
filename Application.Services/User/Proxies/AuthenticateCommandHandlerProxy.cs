using Application.Services.User.CommandHandlers;
using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Repositories;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Application.Services.User.Proxies
{
    [ServiceLocate(typeof(IRequestHandler<AuthenticateRequest, AuthenticateResponse>))]
    public class AuthenticateCommandHandlerProxy : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
    {
        private const string Proxied = nameof(AuthenticateCommandHandler);

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRequestTraceService _requestTraceService;
        private readonly ILogger<AuthenticateCommandHandler> _logger;

        private AuthenticateCommandHandler _handler;

        public AuthenticateCommandHandlerProxy(
            IUserRepository userRepository,
            IConfiguration configuration,
            IRequestTraceService requestTraceService,
            ILogger<AuthenticateCommandHandler> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _requestTraceService = requestTraceService;
            _logger = logger;

            _handler = new AuthenticateCommandHandler(_userRepository, _configuration);
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateRequest request)
        {
            var stopWatch = Stopwatch.StartNew();

            _logger.LogInformation($"Trace Id: {{TraceId}} - Executing {Proxied}", _requestTraceService.TraceId);

            var response = await _handler.Handle(request);

            _logger.LogInformation($"Trace Id: {{TraceId}} - Executed {Proxied}, Time elapsed: {{TimeElapsedInMs}} ms, Reponse: {{ExecutionReponse}}",
                _requestTraceService.TraceId,
                stopWatch.ElapsedMilliseconds,
                JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
