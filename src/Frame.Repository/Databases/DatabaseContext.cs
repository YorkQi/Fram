﻿using Frame.Core.Entitys;
using Frame.Repository.DBContexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Frame.Repository.Databases
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DatabaseContext : IDatabaseContext
    {
        private IServiceProvider provider = default!;
        private DBConnectionString dbConnectionString = default!;

        internal void Initialize(IServiceProvider provider, DBConnectionString dbConnectionString)
        {
            this.provider = provider;
            this.dbConnectionString = dbConnectionString;
        }

        public TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            var serviceScope = provider.CreateScope();
            IDBContext dbContext = serviceScope.ServiceProvider.GetService<IDBContext>() ?? throw new ArgumentNullException(nameof(IDBContext));
            dbContext.Initialize(RandomConnectionString(dbConnectionString));
            var repository = serviceScope.ServiceProvider.GetService<TRepository>() ?? throw new ArgumentNullException(nameof(TRepository));
            repository.Initialize(dbContext);
            return repository;
        }


        public IRepository<TPrimaryKey, TEntity> GetRepository<TPrimaryKey, TEntity>() where TEntity : class, IEntity
        {
            var serviceScope = provider.CreateScope();
            IDBContext dbContext = serviceScope.ServiceProvider.GetService<IDBContext>() ?? throw new ArgumentNullException(nameof(IDBContext));
            dbContext.Initialize(RandomConnectionString(dbConnectionString));
            var repository = new Repository<TPrimaryKey, TEntity>();
            repository.Initialize(dbContext);
            return repository;
        }


        /// <summary>
        /// 随机取得连接串
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static string RandomConnectionString(DBConnectionString connectionString)
        {
            Random random = new();
            var index = random.Next(0, connectionString.Count() - 1);
            var connectionStr = connectionString.Get();
            return connectionStr.ElementAt(index);
        }
    }
}
