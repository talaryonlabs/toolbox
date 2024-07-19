namespace Talaryon.Toolbox;

public interface ITalaryonParams<TResult, out TParams>
{
    ITalaryonRunner<TResult> With(System.Action<TParams> withParams);
}