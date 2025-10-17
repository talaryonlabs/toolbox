namespace Talaryon.Toolbox.Services.Authentik;

public interface IAuthentik
{
    IAuthentikRequestSingle<T> Single<T>() where T : IAuthentikRessource;
    IAuthentikRequestSingle<T> Single<T>(string id) where T : IAuthentikRessource;
    IAuthentikRequestMany<T> Many<T>() where T : IAuthentikRessource;
    // IAuthentikRequest Delete();
    // IAuthentikRequest Update();
    // IAuthentikRequest Create();
}

public interface IAuthentikRessource
{
    
}

public interface IAuthentikRequestSingle<T> : ITalaryonRunner<T> where T : IAuthentikRessource
{
    // public IAuthentikRequestMany<T> WithParam(string key, object value);
}

public interface IAuthentikRequestMany<T> : ITalaryonRunner<AuthentikList<T>> where T : IAuthentikRessource
{
    public IAuthentikRequestMany<T> WithParam(string key, object value);
}