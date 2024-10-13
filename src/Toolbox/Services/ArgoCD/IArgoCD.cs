using System.Threading.Tasks;
using Talaryon.Toolbox.Services.ArgoCD.Models;

namespace Talaryon.Toolbox.Services.ArgoCD;

public interface IArgoCD
{
    public ITalaryonRunner<ArgoCDVersion> GetVersion();
    
    public ITalaryonRunner<V1alpha1AppProjectList> GetProjects(string? name = default);
}