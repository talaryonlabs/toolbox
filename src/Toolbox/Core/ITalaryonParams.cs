namespace Talaryon.Toolbox;

public interface ITalaryonParams<TResult>
{
    ITalaryonRunner<TResult> With(Action<TResult> withParams);
}

public interface ITalaryonParams<TResult, out TParams>
{
    ITalaryonRunner<TResult> With(Action<TParams> withParams);
}