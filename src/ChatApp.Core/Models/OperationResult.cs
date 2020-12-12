using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Models
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }

        public static OperationResult Succeeded => new OperationResult() { IsSuccess = true };
        public static OperationResult Failed => new OperationResult() { IsSuccess = false };
    }
}
