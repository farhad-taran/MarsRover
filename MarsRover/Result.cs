namespace MarsRover
{
    public class Result
    {
        public Error[] Errors { get; }
        public bool IsSuccess { get; }

        protected Result(params Error[] errors)
        {
            Errors = errors;
            IsSuccess = false;
        }

        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public static implicit operator Result(Error error) => new Result(error);
        public static implicit operator Result(Error[] errors) => new Result(errors);

        public static Result Failure(Error error) => new Result(error);
        public static Result Success() => new Result(true);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value) : base(true)
        {
            Value = value;
        }

        protected Result(params Error[] errors) : base(errors) { }
        
        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator Result<T>(Error error) => new Result<T>(error);
        public static implicit operator Result<T>(Error[] errors) => new Result<T>(errors);

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(params Error[] errors) => new Result<T>(errors);
    }
}