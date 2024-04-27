using Application.Services.User.Commands;
using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Application.Services.User.Proxies
{
    [ServiceLocate(typeof(IRequestHandler<RegisterUserRequest, RegisterUserResponse>))]
    public class RegisterUserCommandHandlerProxy : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private const string Proxied = nameof(RegisterUserCommandHandler);

        private readonly IUserPersistor _userPersistor;
        private readonly IRequestTraceService _requestTraceService;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        private RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerProxy(
            IUserPersistor userPersistor,
            IRequestTraceService requestTraceService,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userPersistor = userPersistor;
            _requestTraceService = requestTraceService;
            _logger = logger;

            _handler = new RegisterUserCommandHandler(_userPersistor);
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request)
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
