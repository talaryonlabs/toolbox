namespace Talaryon.Toolbox.Extensions;

public static class UriExtensions
{
    extension(Uri uri)
    {
        public Uri Append(params string[] paths) => new(paths.Aggregate(uri.AbsoluteUri, (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
    }
}