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
    [ServiceLocate(typeof(IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>))]
    public class AddUserClaimCommandHandlerProxy : IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>
    {
        private const string Proxied = nameof(AddUserClaimCommandHandler);

        private readonly IUserRepository _userRepository;
        private readonly IUserPersistor _userPersistor;
        private readonly IRequestTraceService _requestTraceService;
        private readonly ILogger<AddUserClaimCommandHandler> _logger;

        private AddUserClaimCommandHandler _handler;

        public AddUserClaimCommandHandlerProxy(
            IUserRepository userRepository,
            IUserPersistor userPersistor,
            IRequestTraceService requestTraceService,
            ILogger<AddUserClaimCommandHandler> logger)
        {
            _userRepository = userRepository;
            _userPersistor = userPersistor;
            _requestTraceService = requestTraceService;
            _logger = logger;

            _handler = new AddUserClaimCommandHandler(_userRepository, _userPersistor);
        }

        public async Task<AddUserClaimResponse> Handle(AddUserClaimRequest request)
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
