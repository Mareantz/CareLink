namespace Domain.Common
{
	public class Result
	{
		public bool IsSuccess { get; }
		public string ErrorMessage { get; }
		protected Result(bool isSuccess, string errorMessage)
		{
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
		}

		public static Result Success() => new Result(true, null!);

		public static Result Failure(string errorMessage)
		{
			return new Result(false, errorMessage);
		}
	}
	public class Result<T>
	{
		public T Data { get; }
		public bool IsSuccess { get; }
		public string ErrorMessage { get; }
		protected Result(bool isSuccess, T data, string errorMessage)
		{
			IsSuccess = isSuccess;
			Data = data;
			ErrorMessage = errorMessage;
		}

		public static Result<T> Success(T data) => new Result<T>(true, data, null!);

		public static Result<T> Failure(string errorMessage)
		{
			return new Result<T>(false, default!, errorMessage);
		}
	}

}
