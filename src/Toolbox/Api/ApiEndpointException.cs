namespace Talaryon.Toolbox.Api;

public class ApiEndpointException<T>() : Exception($"{nameof(ApiEndpointAttribute)} for {typeof(T).Name} not found.");