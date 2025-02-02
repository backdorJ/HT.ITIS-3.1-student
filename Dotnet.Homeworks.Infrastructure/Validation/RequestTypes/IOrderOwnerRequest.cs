namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IOrderOwnerRequest : IBaseCheck
{
    public Guid OrderId { get; set; }
}