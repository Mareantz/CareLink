using Domain.Common;
using FluentValidation;
using MediatR;

namespace Application
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_validators.Any())
			{
				var context = new ValidationContext<TRequest>(request);

				// Run all validators asynchronously
				var validationResults = await Task.WhenAll(
					_validators.Select(v => v.ValidateAsync(context, cancellationToken))
				);

				var failures = validationResults
					.SelectMany(result => result.Errors)
					.Where(f => f != null)
					.ToList();

				if (failures.Count > 0)
				{
					var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));

					// Determine if TResponse is Result or Result<T>
					if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
					{
						// Get the type argument of Result<T>
						var dataType = typeof(TResponse).GetGenericArguments()[0];

						// Create Result<T>.Failure(string) using reflection
						var failureMethod = typeof(Result<>)
							.MakeGenericType(dataType)
							.GetMethod("Failure", new Type[] { typeof(string) });

						if (failureMethod != null)
						{
							var failureResult = failureMethod.Invoke(null, new object[] { errorMessages });
							return (TResponse)failureResult!;
						}
					}
					else if (typeof(TResponse) == typeof(Result))
					{
						// Create Result.Failure(string)
						var failureResult = Result.Failure(errorMessages);
						return (TResponse)(object)failureResult!;
					}

					// If TResponse is not a Result type, throw exception (optional)
					throw new ValidationException(failures);
				}
			}

			return await next();
		}
	}
}
