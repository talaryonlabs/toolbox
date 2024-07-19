namespace Talaryon.Toolbox;

public interface ITalaryonCreatable<TResult>
{
    ITalaryonRunner<TResult> Create();
}
    
public interface ITalaryonCreatable<TResult, out TParams>
{
    ITalaryonParams<TResult, TParams> Create();
}