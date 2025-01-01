using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Shared.Helpers
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        // Başarılı sonuç döndürmek için
        public static Result<T> SuccessResult(T data)
        {
            return new Result<T> { Success = true, Data = data };
        }

        // Hatalı sonuç döndürmek için
        public static Result<T> FailureResult(List<string> errors)
        {
            return new Result<T> { Success = false, Errors = errors };
        }
    }


    public class Result
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        // Başarılı sonuç döndürmek için
        public static Result SuccessResult()
        {
            return new Result { Success = true };
        }

        // Hatalı sonuç döndürmek için
        public static Result FailureResult(List<string> errors)
        {
            return new Result { Success = false, Errors = errors };
        }
    }
}
