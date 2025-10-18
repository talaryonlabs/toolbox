using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik;

public interface IAuthentik : IApiClient, IApiClientProvider<AuthentikList>
{
    IApiResourceProviderSingle<T> Single<T>() where T : IApiResource;
}