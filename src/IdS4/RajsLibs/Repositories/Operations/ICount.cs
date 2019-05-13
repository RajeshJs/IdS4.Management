﻿using RajsLibs.Key;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.Repositories.Operations
{
    public interface ICount<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        int Count(Expression<Func<TEntity, bool>> predicate = null);
    }

    public interface ILongCount<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        long LongCount(Expression<Func<TEntity, bool>> predicate = null);
    }

    public interface ICountAsync<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ILongCountAsync<TEntity, in TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
