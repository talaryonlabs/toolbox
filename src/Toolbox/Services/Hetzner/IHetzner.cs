namespace Talaryon.Toolbox.Services.Hetzner;

public interface IHetzner
{
    ITalaryonRunner<T[]?> Many<T>() where T : IHetznerObject;
    ITalaryonRunner<T?> Single<T>(string id) where T : IHetznerObject;
}

public interface IHetznerObject
{
    string Name { get; }
}