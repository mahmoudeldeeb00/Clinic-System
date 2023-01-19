using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.Helpers
{
    public class Response<T> where T : class
    {
        public T Data { get; set; }
        public int count { get; set; }
        public int pages { get; set; }
        public string  message { get; set; }
    }
}
