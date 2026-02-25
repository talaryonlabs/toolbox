using System.Reflection;

namespace Talaryon.Toolbox.Extensions;

public static class MemberInfoExtensions
{
    extension(MemberInfo memberInfo)
    {
        public bool HasCustomAttribute<T>() where T : Attribute => memberInfo.GetCustomAttributes<T>(true).Any();
    }
}