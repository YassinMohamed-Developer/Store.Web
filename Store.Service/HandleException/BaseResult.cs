using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.HandleException
{
    public class BaseResult<T>
    {
        public bool IsSucess { get; set; }

        public T Data { get; set; }

        public List<string> Errors { get; set; } = [];


        public BaseResult<T> Sucess(T Value)
        {
            return new BaseResult<T> { Data = Value, IsSucess = true };
        }
        public BaseResult<T> Failure(string Message)
        {
            return new BaseResult<T> { IsSucess = false,Errors = [Message]};
        }
    }
}
