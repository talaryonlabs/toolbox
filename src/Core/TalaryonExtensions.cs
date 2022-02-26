using System.Threading.Tasks;

namespace Talaryon
{
    public static class TalaryonExtensions
    {
        public static T RunSynchronouslyWithResult<T>(this Task<T> task)
        {
            task.RunSynchronously();
            return task.Result;
        }
    }
}