using System;

namespace AllInOne
{
    public interface IGameStateProvider : IService
    {
        event Action<GameState> OnGameStateChanged;
        
        GameState GameState { get; }
        
        void SetGameState(GameState gameState);
    }
}