using System.Reflection;

namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> FindAllImplementations(this Assembly assembly, Type interfaceType)
    {
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException("The specified type must be an interface.", nameof(interfaceType));
        }

        return assembly.GetTypes()
            .Where(type => interfaceType.IsAssignableFrom(type) && type.IsClass);
    }
}