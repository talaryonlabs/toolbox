namespace Talaryon.Toolbox;

public interface ITalaryonUpdatable<TResult>
{
    ITalaryonParams<TResult> Update();
}

public interface ITalaryonUpdatable<TResult, out TParams>
{
    ITalaryonParams<TResult, TParams> Update();
}