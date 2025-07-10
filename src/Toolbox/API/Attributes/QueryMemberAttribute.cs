using Microsoft.AspNetCore.Mvc;

namespace Talaryon.Toolbox.API;

[AttributeUsage(AttributeTargets.Property)]
public class QueryMemberAttribute : FromQueryAttribute
{
    public QueryMemberAttribute(string memberName)
    {
        Name = memberName;
    }
}