using System.Reflection;

namespace Zev.Core.Architecture.Tests;

public static class AssemblyExtensions
{
    public static Assembly[] Exclude(this Assembly[] assemblies, params Assembly[] toExclude)
        => assemblies.ToList().Except(toExclude.ToList()).ToArray();
    
    public static string[] GetNames(this Assembly[] assemblies)
        => assemblies.Select(x => x.GetName().Name).ToArray();
}