namespace Talaryon.Toolbox.Api.Client;

public class ApiEndpointException<T>() : Exception($"{nameof(ApiEndpointAttribute)} for {typeof(T).Name} not found.");