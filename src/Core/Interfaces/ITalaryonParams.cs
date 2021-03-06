namespace Talaryon
{
    public interface ITalaryonParams<TResult, out TParams> : 
        ITalaryonRunner<TResult>
    {
        ITalaryonRunner<TResult> With(System.Action<TParams> withParams);
    }
}