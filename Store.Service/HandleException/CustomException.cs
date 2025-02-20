using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.HandleException
{
    public class CustomException
    {
        public CustomException(int statusCode, string? message = null,string? details = null)
        {
            Details = details;
        }
        public string? Details { get; set; } = null;
    }
}
