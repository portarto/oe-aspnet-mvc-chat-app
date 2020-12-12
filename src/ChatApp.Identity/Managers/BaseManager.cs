using ChatApp.Identity.Core.Assets;
using ChatApp.Identity.Core.Models;
using ChatApp.Identity.Core.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.Managers
{
    public class BaseManager<TModel, TRepository>
        where TModel : class, IBaseModel
        where TRepository : class, IBaseRepository<TModel>
    {
        protected TRepository BaseRepository { get; }

        protected virtual CancellationToken CancellationToken { get; }

        public BaseManager(TRepository baseRepository, ICancellationTokenWrapper cancellationTokenWrapper)
        {
            BaseRepository = baseRepository;
            CancellationToken = cancellationTokenWrapper.CancellationToken;
        }

        public virtual IEnumerable<TModel> GetAll() => BaseRepository.GetAll();

        public virtual TModel FindById(string id) => BaseRepository.FindById(id);

        public virtual Task<int> SaveAsync(TModel model)
            => BaseRepository.SaveAsync(model, CancellationToken);

        public virtual Task<int> UpdateAsync(string id, TModel model)
            => BaseRepository.UpdateAsync(id, model, CancellationToken);

        public virtual Task<int> DeleteAsync(string id)
            => BaseRepository.DeleteAsync(id, CancellationToken);
    }
}
