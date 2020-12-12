using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using ChatApp.Identity.EFCore.DbContexts;
using ChatApp.Identity.EFCore.Entities;
using ChatApp.Identity.EFCore.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Repositories
{
    internal class BaseRepository<TModel, TEntity, TMapper> : IBaseRepository<TModel>, IDisposable
        where TModel : class, IBaseModel
        where TEntity : class, IBaseEntity
        where TMapper : class, IMapper<TModel, TEntity>
    {
        private bool isDisposed;

        protected ChatAppDbContext Context { get; }
        protected TMapper Mapper { get; }
        protected string GetNewId => Guid.NewGuid().ToString();

        public BaseRepository(
            ChatAppDbContext context,
            TMapper mapper
        )
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual Task<int> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entity = GetById(id);
            Context.Set<TEntity>().Remove(entity);
            return Context.SaveChangesAsync(cancellationToken);
        }

        public virtual TModel FindById(string id) => Mapper.MapToModel(GetById(id));

        public virtual IEnumerable<TModel> GetAll() => Entities.Select(e => Mapper.MapToModel(e)).AsEnumerable();

        public virtual Task<int> SaveAsync(TModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            model.Id = GetNewId;
            var entity = Mapper.MapToEntity(model);
            Context.Add(entity);
            return Context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(string id, TModel model, CancellationToken cancellationToken)
        {
            var entity = GetById(id);
            if (entity is not null)
            {
                Mapper.MapForUpdate(model, entity);
                return await Context.SaveChangesAsync(cancellationToken);
            }
            throw new InvalidOperationException("Resource does not exist.");
        }

        protected virtual TEntity GetById(string id) => Context.Set<TEntity>().First(e => e.Id == id);
        protected virtual IQueryable<TEntity> Entities => Context.Set<TEntity>().AsQueryable();

        #region [ DISPOSE ]
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                isDisposed = true;
            }
        }
        #endregion [ DISPOSE ]
    }
}
