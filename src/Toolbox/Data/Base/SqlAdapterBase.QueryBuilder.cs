using System;
using System.Collections.Generic;

namespace TalaryonLabs.Toolbox.Data;

public partial class SqlAdapterBase
{
    private class QueryBuilder<TItem>
    {
        private readonly Dictionary<string, object?> _queryParams = new();
        private readonly string? _table = EntityHelper.GetTableName<TItem>();


        public void Select(string column, string? alias = null) => Select<TItem>(column, alias);

        public void Select<TJoinItem>(string column, string? alias = null)
        {
            if (!_queryParams.ContainsKey("columns"))
                _queryParams.Add("columns", new List<string>());

            ((List<string>)_queryParams["columns"])
                .Add(
                    alias is not null
                        ? $"{EntityHelper.GetTableName<TJoinItem>()}.{column} as {alias}"
                        : $"{EntityHelper.GetTableName<TJoinItem>()}.{column}"
                );
        }

        public void Count()
        {
            if (!_queryParams.ContainsKey("columns"))
                _queryParams.Add("columns", new List<string>());

            ((List<string>)_queryParams["columns"]).Add("COUNT(*)");
        }

        public void Distinct() => _queryParams.TryAdd("distinct", null);

        public void Join<TJoinItem>(string column, string joinedColumn)
        {
            if (!_queryParams.ContainsKey("joins"))
                _queryParams.Add("joins", new List<string>());

            var joinedTable = EntityHelper.GetTableName<TJoinItem>();
            ((List<string>)_queryParams["joins"])
                .Add($"INNER JOIN {joinedTable} ON {joinedTable}.{joinedColumn} = {_table}.{column}");
        }

        public void Where(Action<IDatabaseFilter<TItem>> filter)
        {
            var builder = new SqlQueryFilter<TItem>();
            filter(builder);

            if (!_queryParams.ContainsKey("where"))
                _queryParams.Add("where", builder.BuildFilter());
        }

        public void Limit(int count) => _queryParams.TryAdd("limit", count.ToString());

        public void Offset(int count) => _queryParams.TryAdd("offset", count.ToString());

        public void Order<TJoinItem>(string column, bool asc = true)
        {
            if (!_queryParams.ContainsKey("orders"))
                _queryParams.Add("orders", new List<string>());

            ((List<string>)_queryParams["orders"])
                .Add($"{EntityHelper.GetTableName<TJoinItem>()}.{column} {(asc ? "ASC" : "DESC")}");
        }

        public string Build()
        {
            var query = new List<string?>();

            query.AddRange(new[]
            {
                (
                    _queryParams.ContainsKey("distinct")
                        ? "SELECT DISTINCT"
                        : "SELECT"
                ),
                (
                    _queryParams.TryGetValue("columns", out var columns)
                        ? string.Join(", ", (IEnumerable<string>)columns)
                        : "*"
                ),
                "FROM",
                _table
            });

            if (_queryParams.TryGetValue("joins", out var joins))
                query.Add((string)joins!);

            if (_queryParams.TryGetValue("where", out var where))
                query.AddRange(new[]
                {
                    "WHERE",
                    (string)where!
                });

            if (_queryParams.TryGetValue("orders", out var orders))
                query.AddRange(new[]
                {
                    "ORDER BY",
                    string.Join(", ", (List<string>)orders!)
                });


            if (_queryParams.TryGetValue("limit", out var limit))
                query.AddRange(new[]
                {
                    "LIMIT",
                    (string)limit!
                });

            if (_queryParams.TryGetValue("offset", out var offset))
                query.AddRange(new[]
                {
                    "OFFSET",
                    (string)offset!
                });

            return string.Join(" ", query);
        }
    }
}