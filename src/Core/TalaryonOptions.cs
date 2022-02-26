using Microsoft.Extensions.Options;

namespace Talaryon
{
    public class TalaryonOptions<T> : IOptions<T> 
        where T : class, new()
    {
        T IOptions<T>.Value => (T)(object)this;
    }
}