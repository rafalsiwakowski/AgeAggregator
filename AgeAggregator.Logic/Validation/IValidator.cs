namespace AgeAggregator.Logic.Validation
{
    public interface IValidator<T>
    {
        bool IsValid(T instance);
    }
}
