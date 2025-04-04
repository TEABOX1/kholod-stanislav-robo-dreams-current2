using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace AllInOne
{
    public class GameStateController : GlobalMonoServiceBase, IGameStateProvider
    {
        public event Action<GameState> OnGameStateChanged; 
        
        private StateMachine _stateMachine;
        
        public GameState GameState
        {
            get => (GameState)_stateMachine.CurrentState.StateId;
            set
            {
                _stateMachine.SetState((byte)value);
                OnGameStateChanged?.Invoke(value);
            }
        }
        
        public override Type Type { get; } = typeof(IGameStateProvider);

        private void Start()
        {
            ISceneManager sceneManager = ServiceLocator.Instance.GetService<ISceneManager>();
            
            _stateMachine = new StateMachine();
            _stateMachine.AddState((byte)GameState.MainMenu, new MainMenu(_stateMachine, (byte)GameState.MainMenu, sceneManager, Scenes.MainMenu));
            _stateMachine.AddState((byte)GameState.Gameplay, new Gameplay(_stateMachine, (byte)GameState.Gameplay, sceneManager, Scenes.Gameplay));
            _stateMachine.AddState((byte)GameState.Paused, new Pause(_stateMachine, (byte)GameState.Paused, sceneManager, Scenes.Gameplay));
        }

        public void SetGameState(GameState gameState)
        {
            GameState = gameState;
        }
    }
}