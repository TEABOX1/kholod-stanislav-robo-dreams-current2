using UnityEngine;
using UnityEngine.SceneManagement;

namespace AllInOne
{
    public class SceneState : StateBase
    {
        private readonly string _sceneName;
        private readonly ISceneManager _sceneManager;
        
        public SceneState(StateMachine stateMachine, byte stateId, ISceneManager sceneManager, string sceneName) : base(stateMachine, stateId)
        {
            _sceneName = sceneName;
            _sceneManager = sceneManager;
        }

        public override void Enter()
        {
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Single);
            _sceneManager.OnSceneLoad(asyncOperation);
        }

        public override void Dispose()
        {
        }
    }
}