using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.Toolbox.Services.Caching;

public class CacheService :
    ICacheService,
    ITalaryonRunner
{
    private IDistributedCache Cache { get; }
    private DistributedCacheEntryOptions EntryOptions { get; }

    private IEnumerable<string> _deleteKeys;

    public CacheService(IDistributedCache cache)
    {
        _deleteKeys = new List<string>();
        Cache = cache;
        EntryOptions = new DistributedCacheEntryOptions();
    }

    ICacheServiceEntry<T> ICacheService.Key<T>(string key)
    {
        return new ServiceEntry<T>(this, key);
    }

    public ICacheServiceEntry<object> Key(string key)
    {
        return new ServiceEntry<object>(this, key);
    }

    ITalaryonRunner ICacheService.RemoveMany(IEnumerable<string> keys)
    {
        _deleteKeys = keys;
        return this;
    }
    void ITalaryonRunner.Run() =>  (this as ITalaryonRunner)
        .RunAsync()
        .RunSynchronously();

    Task ITalaryonRunner.RunAsync(CancellationToken cancellationToken)
    {
        return Task.WhenAll(_deleteKeys
            .Select(key => Cache.RemoveAsync(key, cancellationToken))
        );
    }

    private class ServiceEntry<T> :
        ICacheServiceEntry<T>,
        ITalaryonRunner
    {
        private readonly CacheService _service;
        private readonly string _key;

        private T? _value;
        private bool _remove;
        private bool _refresh;

        public ServiceEntry(CacheService service, string key)
        {
            _service = service ?? throw new NullReferenceException();
            _key = key ?? throw new NullReferenceException();
            _value = default;
        }

        T? ITalaryonRunner<T>.Run() => (this as ITalaryonRunner<T?>)
            .RunAsync()
            .RunSynchronouslyWithResult();

        async Task<T?> ITalaryonRunner<T>.RunAsync(CancellationToken cancellationToken)
        {
            var data = await _service
                .Cache
                .GetAsync(_key, cancellationToken);

            if (data is null || data.Length == 0)
                return default;

            return TalaryonHelper.DeserializeObject<T>(data);
        }


        ITalaryonRunner ICacheServiceEntry<T>.Set(T? value)
        {
            _value = value ?? throw new NullReferenceException();
            return this;
        }

        ITalaryonRunner ICacheServiceEntry<T>.Refresh(T value)
        {
            _refresh = true;
            return this;
        }

        ITalaryonRunner ITalaryonDeletable.Delete(bool force)
        {
            _remove = true;
            return this;
        }

        void ITalaryonRunner.Run() => (this as ITalaryonRunner)
            .RunAsync()
            .RunSynchronously();

        Task ITalaryonRunner.RunAsync(CancellationToken cancellationToken) =>
            _remove
                ? _service
                    .Cache
                    .RemoveAsync(
                        _key,
                        cancellationToken
                    )
                : _refresh
                    ? _service
                        .Cache
                        .RefreshAsync(_key, cancellationToken)
                    : _service
                        .Cache
                        .SetAsync(
                            _key,
                            TalaryonHelper.SerializeObject(_value),
                            _service.EntryOptions,
                            cancellationToken
                        );

        ITalaryonRunner<bool> ITalaryonExistable.Exists() => 
            new ServiceEntryCheck(_service, _key);
    }

    private class ServiceEntryCheck :
        ITalaryonRunner<bool>
    {
        private readonly CacheService _service;
        private readonly string _key;

        public ServiceEntryCheck(CacheService service, string key)
        {
            _service = service ?? throw new NullReferenceException();
            _key = key ?? throw new NullReferenceException();
        }
            
        bool ITalaryonRunner<bool>.Run() => (this as ITalaryonRunner<bool>)
            .RunAsync()
            .RunSynchronouslyWithResult();

        async Task<bool> ITalaryonRunner<bool>.RunAsync(CancellationToken cancellationToken)
        {
            var value = await _service
                .Cache
                .GetAsync(_key, cancellationToken);
            return value is not null && value.Length != 0;
        }
    }
}