using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Talaryon.Toolbox.API.Hosting;

[ApiController]
public abstract class ApiController<T, TArgs> : ControllerBase 
    where TArgs : ApiListArgs
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
    protected abstract Task<ApiList<T>> List([FromQuery] TArgs args, CancellationToken cancellationToken);
    
    
    [HttpGet("{itemId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
    protected abstract Task<T> Get([FromRoute] string itemId, CancellationToken cancellationToken);
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
    [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
    protected abstract Task<T> Create([FromBody] ApiRequest<T> request, CancellationToken cancellationToken);
    
    [HttpPatch("{itemId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
    protected abstract Task<T> Update([FromRoute] string itemId, [FromBody] ApiRequest<T> request, CancellationToken cancellationToken);
    
    [HttpDelete("{itemId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
    [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
    protected abstract Task<T> Delete([FromRoute] string itemId, CancellationToken cancellationToken);
}