using System;
using UnityEngine;

namespace AllInOne
{
    public class SceneManager : GlobalMonoServiceBase, ISceneManager
    {
        [Serializable]
        private struct SceneInfo
        {
            public Scenes scene;
            public string sceneName;
        }
        
        public event Action<AsyncOperation> onSceneLoad; 
        
        [SerializeField] private SceneInfo[] _scenes;
        
        private StateMachine _stateMachine;
        
        public override Type Type { get; } = typeof(ISceneManager);

        private void Start()
        {
            _stateMachine = new StateMachine();

            for (int i = 0; i < _scenes.Length; ++i)
            {
                SceneInfo sceneInfo = _scenes[i];
                _stateMachine.AddState((byte)sceneInfo.scene, new SceneState(_stateMachine, (byte)sceneInfo.scene, this, sceneInfo.sceneName));
            }
        }

        public void SetScene(Scenes scene)
        {
            _stateMachine.SetState((byte)scene);
        }

        public void OnSceneLoad(AsyncOperation asyncOperation)
        {
            onSceneLoad?.Invoke(asyncOperation);
        }
    }
}