namespace Talaryon.Toolbox.Services.ArgoCD;


public class V1alpha1AppProjectParams : TalaryonParams
{
    private Dictionary<string, string> _meta = new();
    private Dictionary<string, string> _spec = new();
    
    public V1alpha1AppProjectParams()
    {
        BaseDictionary.Add("metadata", _meta);
        BaseDictionary.Add("spec", _spec);
    }

    public V1alpha1AppProjectParams Name(string name)
    {
        if(!_meta.TryAdd("name", name)) _meta["name"] = name;
        return this;
    }

    public V1alpha1AppProjectParams Description(string description)
    { 
        if(!_spec.TryAdd("description", description)) _spec["description"] = description;
        return this;
    }
}