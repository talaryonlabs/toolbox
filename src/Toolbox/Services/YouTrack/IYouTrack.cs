using System;
using System.Collections.Generic;

namespace TalaryonLabs.Toolbox.Services.YouTrack;

public interface IYouTrack
{
    IYouTrackRessouceProvider<YouTrackProject> Project(string id);
    IYouTrackRessoucesProvider<YouTrackProject> Projects();
}

public interface IYouTrackRessouceProvider<T> : ITalaryonRunner<T>, ITalaryonExistable
    where T : IYouTrackRessource
{
    
}

public interface IYouTrackRessoucesProvider<T> : ITalaryonEnumerable<T>, ITalaryonCountable
    where T : IYouTrackRessource
{
    
}

public interface IYouTrackRessource
{
    string? Id { get; }
}


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class YouTrackApiEndpointAttribute(string? url, YouTrackApiEndpointType type = YouTrackApiEndpointType.List) : Attribute
{
    public string? Url { get; set; } = url;
    public YouTrackApiEndpointType Type { get; set; } = type;
}

public enum YouTrackApiEndpointType
{
    List,
    Get,
    Create,
    Update,
    Delete
}


public interface IYouTrackCustomField
{
    string Id { get; }
    string Name { get; set; }
}

public interface IYouTrackCustomFieldable<T>
    where T : IYouTrackCustomField
{
    List<T> CustomFields { get; }
}