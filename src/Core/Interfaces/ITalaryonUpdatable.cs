namespace Talaryon
{
    public interface ITalaryonUpdatable<TResult, out TParams>
    {
        ITalaryonParams<TResult, TParams> Update();
    }
}