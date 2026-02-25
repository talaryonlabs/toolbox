using Microsoft.AspNetCore.Mvc;

namespace Talaryon.Toolbox.Hosting.Api;

[AttributeUsage(AttributeTargets.Property)]
public class QueryMemberAttribute : FromQueryAttribute
{
    public QueryMemberAttribute(string memberName)
    {
        Name = memberName;
    }
}