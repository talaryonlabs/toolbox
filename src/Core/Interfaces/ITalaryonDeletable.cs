namespace Talaryon
{
    public interface ITalaryonDeletable
    {
        ITalaryonRunner Delete(bool force = false);
    }
    
    public interface ITalaryonDeletable<TResult>
    {
        ITalaryonRunner<TResult> Delete(bool force = false);
    }
}