using System;
using System.Linq;
using System.Reflection;

namespace Talaryon.Toolbox.Extensions;

public static class MemberInfoExtensions
{
    public static bool HasCustomAttribute<T>(this MemberInfo memberInfo) where T : Attribute => memberInfo.GetCustomAttributes<T>(true).Any();
}