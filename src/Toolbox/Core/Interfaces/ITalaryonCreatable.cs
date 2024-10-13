namespace Talaryon.Toolbox;

public interface ITalaryonCreatable<TResult>
{
    ITalaryonParams<TResult> Create();
}
    
public interface ITalaryonCreatable<TResult, out TParams>
{
    ITalaryonParams<TResult, TParams> Create();
}