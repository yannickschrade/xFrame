namespace xFrame.Core.Validation
{
    public interface IValidatorContext<T>
    {
        void AddCondition(ICondition condition);
    }
}
