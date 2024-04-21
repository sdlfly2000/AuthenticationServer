﻿using Common.Core.DependencyInjection;

namespace Infra.Core.RequestTrace
{
    [ServiceLocate(typeof(IRequestTraceService), ServiceType.Scoped)]
    public class RequestTraceService : IRequestTraceService
    {
        public string TraceId { get; set; }
    }
}
