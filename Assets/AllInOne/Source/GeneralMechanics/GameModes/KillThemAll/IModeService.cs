using System;

namespace AllInOne
{
    public interface IModeService : IService
    {
        event Action<bool> OnComplete;
        
        void Begin();
    }
}