using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.EFCore.Entities
{
    internal interface IBaseEntity
    {
        string Id { get; set; }
    }
}
