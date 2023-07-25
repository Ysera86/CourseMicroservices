using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtoss
{
    public class ResponseDto<T>
    {
        public T Data { get; private set; }

        [JsonIgnore] // statuscode zaten response ile dönüyor, bunu içeride kendim kullanıcam, response bodysinde bir daha belirtmeme gerek yok.
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; set; }

        //Static Factory Method
        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        } 
        
        public static ResponseDto<T> Fail(List<string> errors,int statusCode)
        {
            return new ResponseDto<T> { Data = default, StatusCode = statusCode, IsSuccessful = false, Errors = errors };
        }

        public static ResponseDto<T> Fail(string error,int statusCode)
        {
            return new ResponseDto<T> { Data = default, StatusCode = statusCode, IsSuccessful = false, Errors = new List<string> { error} };
        }
    }
}
