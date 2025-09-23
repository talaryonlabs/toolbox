using Newtonsoft.Json;

namespace Talaryon.Toolbox.Services.Directus;

public enum DirectusFilterOperator
{
    And,
    Or
}

public interface IDirectusFilter
{
    IDirectusFilter Group(DirectusFilterOperator groupOperator, Action<IDirectusFilter> filterBuilder);
    IDirectusFilter Equals(string field, string value);
    IDirectusFilter NotEquals(string field, string value);
    
    IDirectusFilter GreaterThan(string field, string value);
    IDirectusFilter GreaterThanOrEqual(string field, string value);
    IDirectusFilter LessThan(string field, string value);
    IDirectusFilter LessThanOrEqual(string field, string value);
    
    IDirectusFilter In(string field, string[] values);
    IDirectusFilter NotIn(string field, string[] values);
    IDirectusFilter Contains(string field, string value);
    IDirectusFilter NotContains(string field, string value);
    
    IDirectusFilter StartsWith(string field, string value);
    IDirectusFilter EndsWith(string field, string value);
    
    string Build();
}

public class DirectusFilterBuilder : IDirectusFilter
{
    class Filter
    {
        public required string Field { get; init; }
        public required string Operator { get; init; }
        public required object Value { get; init; }

        public static implicit operator Dictionary<string, object>(Filter filter)
        {
            return new Dictionary<string, object>
            {
                { filter.Field, new Dictionary<string, object> { { filter.Operator, filter.Value } } }
            };
        }
    }

    class FilterGroup
    {
        public required string Operator { get; init; }
        public required List<Dictionary<string, object>> Filters { get; init; }
        
        public static implicit operator Dictionary<string, object>(FilterGroup group)
        {
            return new Dictionary<string, object> { { group.Operator, group.Filters } };
        }
    }
    
    private List<Dictionary<string, object>> List { get; } = new();

    public IDirectusFilter Group(DirectusFilterOperator groupOperator, Action<IDirectusFilter> filterBuilder)
    {
        var filter = new DirectusFilterBuilder();
        filterBuilder.Invoke(filter);
        
        var group = new FilterGroup
        {
            Operator = $"_{groupOperator.ToString().ToLower()}",
            Filters = filter.List
        };
        
        List.Add(group);

        return this;
    }

    public IDirectusFilter Equals(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_eq", Value = value });
        return this;
    }

    public IDirectusFilter NotEquals(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_neq", Value = value });
        return this;
    }

    public IDirectusFilter GreaterThan(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_gt", Value = value });
        return this;
    }

    public IDirectusFilter GreaterThanOrEqual(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_gte", Value = value });
        return this;
    }

    public IDirectusFilter LessThan(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_lt", Value = value });
        return this;
    }

    public IDirectusFilter LessThanOrEqual(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_lte", Value = value });
        return this;
    }

    public IDirectusFilter In(string field, string[] values)
    {
        List.Add(new Filter { Field = field, Operator = "_in", Value = values });
        return this;
    }

    public IDirectusFilter NotIn(string field, string[] values)
    {
        List.Add(new Filter { Field = field, Operator = "_nin", Value = values });
        return this;
    }

    public IDirectusFilter Contains(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_contains", Value = value });
        return this;
    }

    public IDirectusFilter NotContains(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_ncontains", Value = value });
        return this;
    }

    public IDirectusFilter StartsWith(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_starts_with", Value = value });
        return this;
    }

    public IDirectusFilter EndsWith(string field, string value)
    {
        List.Add(new Filter { Field = field, Operator = "_ends_with", Value = value });
        return this;
    }

    public string Build()
    {
        return JsonConvert.SerializeObject(List is { Count: 1 } ? List.First() : List, Formatting.None);
    }
}