namespace Talaryon.Toolbox.Extensions;

public static class TaskExtensions
{
    extension<T>(Task<T> task)
    {
        public T RunSynchronouslyWithResult()
        {
            task
                .Wait();
            return task.Result;
        }
    }
    
    extension<T>(ValueTask<T> valueTask)
    {
        public T RunSynchronouslyWithResult()
        {
            valueTask
                .AsTask()
                .Wait();
        
            return valueTask.Result;
        }
    }
}