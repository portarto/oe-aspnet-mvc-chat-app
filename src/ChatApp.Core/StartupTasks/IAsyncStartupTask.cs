using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.StartupTasks
{
    public interface IAsyncStartupTask
    {
        int Order { get; }
        Task ExecuteAsync();
    }
}
