using ChatApp.Identity.Core.Models;
using ChatApp.Identity.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Mapper
{
    internal interface IMapper<TModel, TEntity>
        where TModel : class, IBaseModel
        where TEntity : class, IBaseEntity
    {
        TModel MapToModel(TEntity entity);
        TEntity MapToEntity(TModel model);
        void MapForUpdate(TModel updatedModel, TEntity original);
    }
}
