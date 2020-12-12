using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    [System.Serializable]
    public class ApiResponse<T>
    {
        public string type;
        public T data;
    }
}
