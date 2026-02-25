using Talaryon.Toolbox.Api.Client;

namespace Talaryon.Toolbox.Services.Authentik;

public interface IAuthentik : IApiClient, IApiClientProvider<AuthentikList>
{
    IApiResourceProviderSingle<T> Single<T>() where T : IApiResource;
}