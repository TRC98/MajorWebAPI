using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Core.Model
{
    public class WebAPICommonResponse
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }
        public string Message { get; set; }

    }
}
