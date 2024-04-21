using Application.Services.User.CommandHandlers;
using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Infra.Core.RequestTrace;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Application.Services.User.Proxies
{
    [ServiceLocate(typeof(IRequestHandler<UpdateUserClaimRequest, UpdateUserClaimResponse>))]
    public class UpdateUserClaimCommandHandlerProxy : IRequestHandler<UpdateUserClaimRequest, UpdateUserClaimResponse>
    {
        private const string Proxied = nameof(UpdateUserClaimCommandHandler);

        private readonly IUserRepository _userRepository;
        private readonly IUserPersistor _userPersistor;
        private readonly IRequestTraceService _requestTraceService;
        private readonly ILogger<UpdateUserClaimCommandHandler> _logger;

        private UpdateUserClaimCommandHandler _handler;

        public UpdateUserClaimCommandHandlerProxy(
            IUserRepository userRepository,
            IUserPersistor userPersistor,
            IRequestTraceService requestTraceService,
            ILogger<UpdateUserClaimCommandHandler> logger)
        {
            _userPersistor = userPersistor;
            _userRepository = userRepository;
            _requestTraceService = requestTraceService;
            _logger = logger;

            _handler = new UpdateUserClaimCommandHandler(_userRepository, _userPersistor);
        }

        public async Task<UpdateUserClaimResponse> Handle(UpdateUserClaimRequest request)
        {
            var stopWatch = Stopwatch.StartNew();

            _logger.LogInformation($"Trace Id: {{RequsetId}} - Executing {Proxied}", _requestTraceService.TraceId);

            var response = await _handler.Handle(request);

            _logger.LogInformation($"Trace Id: {{RequestId}} - Executed {Proxied}, Time elapsed: {{TimeElapsedInMs}} ms, Reponse: {{ExecutionReponse}}",
                _requestTraceService.TraceId,
                stopWatch.ElapsedMilliseconds,
                JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
