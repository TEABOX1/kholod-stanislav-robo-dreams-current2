using System;

namespace AllInOne
{
    public interface IState : IDisposable
    {
        byte StateId { get; }
        
        void Enter();
        void Update(float deltaTime);
        void Exit();
    }
}