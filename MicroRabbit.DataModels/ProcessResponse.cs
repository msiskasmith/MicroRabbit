using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.DataModels
{
    public class ProcessResponse
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsErrorOccurred { get; set; }    
        public object Object { get; set; }
    }
}
