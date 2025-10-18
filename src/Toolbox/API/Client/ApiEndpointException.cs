namespace Talaryon.Toolbox.API.Client;

public class ApiEndpointException<T>() : Exception($"AuthentikApiEndpoint for {typeof(T).Name} not found.");