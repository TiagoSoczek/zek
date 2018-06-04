namespace Zek.Shared
{
    public enum ResultCode
    {
        Unknown = 0,
        Ok = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        NotFound = 404,
        UnprocessableEntity = 422,
        InternalServerError = 500
    }

    public interface IResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
        string Error { get; }
        ResultCode Code { get; }

        object GetObjectValue();
        bool HasValue { get; }
    }

    public sealed partial class Result<T> : IResult
    {
        public T Value { get; }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }
        public ResultCode Code { get; }

        public object GetObjectValue() => Value;
        public bool HasValue => Value != null;
    }

    public sealed partial class Result : IResult
    {
        public object Value { get; }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }
        public ResultCode Code { get; }

        public object GetObjectValue() => Value;
        public bool HasValue => Value != null;
    }
}