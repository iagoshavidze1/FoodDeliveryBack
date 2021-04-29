using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class Result
    {
        public bool Success { get; set; }

        public List<string> Errors { get; set; }

        public static Result Fail(string error) => new Result { Errors = new List<string> { error } };

        public static Result Fail(List<string> errors) => new Result { Errors = errors.ToList() };

        public static Result Succeed() => new Result { Success = true };
    }
}
