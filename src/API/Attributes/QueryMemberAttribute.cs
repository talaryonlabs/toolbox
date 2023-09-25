using System;
using Microsoft.AspNetCore.Mvc;

namespace TalaryonLabs.Toolbox.API;

[AttributeUsage(AttributeTargets.Property)]
public class QueryMemberAttribute : FromQueryAttribute
{
    public QueryMemberAttribute(string memberName)
    {
        Name = memberName;
    }
}