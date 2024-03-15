using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace SeaFoodShop.DataContext.Models
{


    public class MethodResult
    {
        public string? Message { get; set; }
        public bool? IsSuccess { get; set; }
        public object? Data { get; set; }
        public List<object>? ListObject { get; set; }   
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
        public static MethodResult Result(object? data, string? message)
        {
            return new MethodResult
            {
                Data = data,
                Message = message
            };
        }
        public static MethodResult Result(List<object>? listObject, string? message)
        {
            return new MethodResult
            {
                ListObject = listObject,
                Message = message
            };
        }
    } 

}
