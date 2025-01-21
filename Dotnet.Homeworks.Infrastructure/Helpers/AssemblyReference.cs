using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Helpers;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}