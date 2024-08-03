using System.Collections.Generic;

namespace Talaryon.Toolbox.Services.Hetzner;

public interface IHetzner
{
    ITalaryonRunner<T[]?> Many<T>() where T : IHetznerObject;
    ITalaryonRunner<T?> Single<T>(string id) where T : IHetznerObject;
    ITalaryonRunner<T?> Single<T>(int id) where T: IHetznerObject => Single<T>(id.ToString());
}

public interface IHetznerObject
{
    string Name { get; }
}