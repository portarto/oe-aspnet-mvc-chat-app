using ChatApp.Identity.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp.Identity.Core.Repositories
{
    public interface IBaseRepository<TModel> where TModel : class, IBaseModel
    {
        IEnumerable<TModel> GetAll();

        TModel FindById(string id);

        Task<int> SaveAsync(TModel model, CancellationToken cancellationToken);

        Task<int> UpdateAsync(string id, TModel model, CancellationToken cancellationToken);

        Task<int> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
