namespace Zek.Shared
{
    public partial class Result
    {
        public static readonly Result Success = new Result();
        public static readonly Result NotFound = new Result(isSuccess: false, error: "Item not found", code: ResultCode.NotFound);

        public static Result Fail(string error, ResultCode code = ResultCode.BadRequest) => new Result(isSuccess: false, error: error, code: code);
        public static Result<T> Ok<T>(T value, ResultCode code = ResultCode.Ok) => new Result<T>(value, code: code);
        public static Result<T> Fail<T>(string error, ResultCode code = ResultCode.BadRequest) => new Result<T>(default(T), false, error, code);

        public static implicit operator bool(Result result)
        {
            return result.IsSuccess;
        }

        public static implicit operator Result(bool success)
        {
            if (success)
            {
                return Success;
            }

            return Fail(null);
        }

        public Result(object value = default(object), bool isSuccess = true, string error = null, ResultCode code = ResultCode.Ok)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
            Code = code;
        }
    }

    public partial class Result<T>
    {
        public static readonly Result<T> NotFound = new Result<T>(default(T), false, "Item not found", ResultCode.NotFound);

        public static implicit operator bool(Result<T> result)
        {
            return result.IsSuccess;
        }

        public static implicit operator Result<T>(T value)
        {
            if (value == null)
            {
                return NotFound;
            }

            return Result.Ok(value);
        }

        public static implicit operator Result<T>(Result result)
        {
            if (result.IsFailure)
            {
                return Result.Fail<T>(result.Error, result.Code);
            }

            var resultValue = default(T);

            if (result.HasValue)
            {
                resultValue = (T)result.Value;
            }

            return new Result<T>(resultValue, result.IsSuccess, result.Error, result.Code);
        }

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsFailure)
            {
                return Result.Fail(result.Error, result.Code);
            }

            return new Result(result.Value, result.IsSuccess, result.Error, result.Code);
        }

        public Result(T value = default(T), bool isSuccess = true, string error = null, ResultCode code = ResultCode.Ok)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
            Code = code;
        }
    }
}