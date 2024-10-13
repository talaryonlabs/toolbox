using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Talaryon.Toolbox.Extensions;
using Talaryon.Toolbox.Services.ArgoCD.Models;

namespace Talaryon.Toolbox.Services.ArgoCD;

public class ArgoFactory<T>(HttpClient httpClient, string? id = null) :
        IArgoResourceCreateFactory<T>,
        IArgoResourceUpdateFactory<T>,
        ITalaryonRunner<T>
{
    private static MediaTypeHeaderValue MediaType => new("application/json");
    
    private T? _obj;
    private bool _update, _create;
    private Action<T>? _action;
    
    public ITalaryonParams<T> Create()
    {
        _obj = Activator.CreateInstance<T>();
        _create = true;
        return this;
    }
        
    public ITalaryonParams<T> Update()
    {
        _obj = default;
        _update = true;
        return this;
    }
    
    public ITalaryonRunner<bool> Delete(bool force = false) => new DeleteResource(httpClient, id, force);

    ITalaryonRunner<T> ITalaryonParams<T>.With(Action<T> withParams)
    {
        _action = withParams;
        return this;
    }

    public async Task<T?> RunAsync(CancellationToken cancellationToken = default)
    {
        var endpoint =
            ((_create
                 ? ArgoEndpoint.GetEndpoint<T>(ArgoEndpointType.Create)
                 : (_update
                     ? ArgoEndpoint.GetEndpoint<T>(ArgoEndpointType.Update)
                     : null)) ??
             throw new ArgoEndpointException())
            .TrimStart('/')
            .Replace(".id", id);

        if (_update)
        {
            var getEndpoint = (ArgoEndpoint.GetEndpoint<T>(ArgoEndpointType.Get) ?? throw new ArgoEndpointException())
                .TrimStart('/')
                .Replace(".id", id);
            try
            {
                _obj = await httpClient.GetFromJsonAsync<T>(getEndpoint, cancellationToken);
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<ArgoFactory<T>>(e.Message);
                return default;
            }
        }
        _action?.Invoke(_obj!);
        
        
        var fields = ArgoEndpoint.GetFields<T>()
            .Concat(ArgoEndpoint.GetAdditionalFields<T>())
            .ToList();
        

        var query = new QueryBuilder();
        var requestUri = $"{endpoint}{query.ToQueryString()}";

        var container = ArgoContainer.GetName<T>() ?? throw new ArgoContainerException();
        var requestBody = new Dictionary<string, object?> { { container, _obj } };
        
        var content = JsonContent.Create(requestBody);
        content.Headers.ContentType = MediaType;
        
        try
        {
            TalaryonLogger.Debug<ArgoFactory<T>>($"[{(_create ? "CREATE" : "UPDATE")}] {requestUri}");
            if (_create)
            {
                var createResponse = await httpClient.PostAsync(requestUri, content, cancellationToken);
                return await createResponse.Content.ReadFromJsonAsync<T>(cancellationToken);
            }
            
            var response = await httpClient.PutAsync(requestUri, content, cancellationToken);
            return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        }
        catch (Exception e)
        {
            TalaryonLogger.Error<ArgoFactory<T>>(e.Message);
            return default;
        }
    }
    T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T>).RunAsync().RunSynchronouslyWithResult();

    /***
     * Internal class to delete a specific resource
     */
    class DeleteResource(HttpClient httpClient, string? id, bool force) : ITalaryonRunner<bool>
    {
        public bool Force { get; } = force;

        public async Task<bool> RunAsync(CancellationToken cancellationToken = default)
        {
            var endpoint = (ArgoEndpoint.GetEndpoint<T>(ArgoEndpointType.Delete) ?? throw new ArgoEndpointException())
                .TrimStart('/')
                .Replace(".id", id);
            
            var message = new HttpRequestMessage(HttpMethod.Delete, endpoint)
            {
                Content = JsonContent.Create(new Dictionary<string, object>())
            };
            message.Content.Headers.ContentType = MediaType;
            
            try
            {
                TalaryonLogger.Debug<ArgoFactory<T>>($"[DELETE] {endpoint}");
                return (await httpClient.SendAsync(message, cancellationToken)).StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                TalaryonLogger.Error<ArgoFactory<T>>(e.Message);
                return default;
            }
        }
        public bool Run() => RunAsync().RunSynchronouslyWithResult();
    }
}