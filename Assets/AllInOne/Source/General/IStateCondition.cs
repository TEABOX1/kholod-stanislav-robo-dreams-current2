namespace AllInOne
{
    public interface IStateCondition
    {
        byte State { get; }
        
        bool Invoke();
    }
}