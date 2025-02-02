namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IClientRequest : IBaseCheck
{
    public Guid Guid { get; }
}