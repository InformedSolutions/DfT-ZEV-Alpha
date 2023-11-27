using System.Reflection;

namespace DfT.ZEV.Core.Architecture.Tests;

public static class AssemblyExtensions
{
    public static Assembly[] Exclude(this Assembly[] assemblies, params Assembly[] toExclude)
    {
        return assemblies.ToList().Except(toExclude.ToList()).ToArray();
    }

    public static string[] GetNames(this Assembly[] assemblies)
    {
        return assemblies.Select(x => x.GetName().Name).ToArray()!;
    }
}