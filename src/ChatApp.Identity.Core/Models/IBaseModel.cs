using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Identity.Core.Models
{
    public interface IBaseModel
    {
        public string Id { get; set; }
    }
}
