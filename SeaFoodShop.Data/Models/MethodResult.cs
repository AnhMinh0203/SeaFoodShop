using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{


    public class MethodResult
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public MethodResult() { }
        public static MethodResult ResultWithError(string message, bool isSuccess)
        {
            return new MethodResult
            {
                Message = message,
                IsSuccess = isSuccess
            };
        }
        public static MethodResult ResultWithSuccess(string message, bool isSuccess)
        {
            return new MethodResult
            {
                Message = message,
                IsSuccess = isSuccess
            };
        }
    } 

}
