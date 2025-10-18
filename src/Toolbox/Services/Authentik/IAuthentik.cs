using Talaryon.Toolbox.API.Client;

namespace Talaryon.Toolbox.Services.Authentik;

public interface IAuthentik : IApiClient
{
    IApiResourceProviderSingle<T> Single<T>() where T : IApiResource;
}