using ArxOne.MrAdvice.Advice;
using Infra.Core.Extentions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Infra.Core.Cache;

public class CacheAttribute : Attribute, IMethodAsyncAdvice
{
    private string _key;
    private Type _subKeyType;

    public CacheAttribute(string key, Type subKeyType)
    {
        _key = key;
        _subKeyType = subKeyType;
    }

    public async Task Advise(MethodAsyncAdviceContext context)
    {
        var serviceProvider = context.GetMemberServiceProvider();

        var memoryCache = serviceProvider?.GetRequiredService<IMemoryCache>();

        var request = context.Arguments?.SingleOrDefault(argument => argument.GetType() == _subKeyType);

        var subKey = (request as dynamic).Id as string;

        if (!string.IsNullOrEmpty(subKey) 
            && memoryCache.TryGetValue(_key + subKey, out var value))
        {
            context.ReturnValue = Task.FromResult(value);
            return;
        }

        await context.ProceedAsync();

        var result = (context.ReturnValue as dynamic).Result;

        if (!string.IsNullOrEmpty(subKey))
        {
            memoryCache?.Set(_key + subKey, (object)result);
        }
    }
}
