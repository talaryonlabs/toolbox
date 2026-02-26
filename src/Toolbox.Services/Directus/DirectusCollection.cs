namespace Talaryon.Toolbox.Services.Directus;

public class DirectusCollection<T> : List<Dictionary<string, T>>
{
    public IReadOnlyList<T> ToList() => this.SelectMany(x => x.Values).ToList();
}