using System.Threading.Tasks;

namespace Talaryon.Toolbox.Extensions;

public static class TaskExtensions
{
    public static T RunSynchronouslyWithResult<T>(this Task<T> task)
    {
        task
            .Wait();
        return task.Result;
    }
    
    public static T RunSynchronouslyWithResult<T>(this ValueTask<T> valueTask)
    {
        valueTask
            .AsTask()
            .Wait();
        
        return valueTask.Result;
    }
}