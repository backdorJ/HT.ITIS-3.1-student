namespace Dotnet.Homeworks.Shared.Dto;

public class ResultFactory
{
    public static TResponse CreateResult<TResponse>(bool isSuccess, object? value = default, string? error = default)
    {
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericArgs = typeof(TResponse).GetGenericArguments();
            var resultType = typeof(Result<>).MakeGenericType(genericArgs);

            var constructor = resultType.GetConstructor(new[] { genericArgs[0], typeof(bool), typeof(string) });
            if (constructor == null)
            {
                throw new InvalidOperationException($"Constructor not found for {resultType}");
            }

            var result = constructor.Invoke(new[] { value, isSuccess, error });
            return (TResponse)result;
        }

        return (TResponse)(object)new Result(isSuccess, error);
    }
}