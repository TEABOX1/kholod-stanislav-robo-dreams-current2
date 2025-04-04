
namespace AllInOne
{
    public abstract class GameStateBase : StateBase
    {
        protected readonly ISceneManager _sceneManager;
        protected readonly Scenes _scene;
        
        protected GameStateBase(StateMachine stateMachine, byte stateId, ISceneManager sceneManager, Scenes scene) : base(stateMachine, stateId)
        {
            _sceneManager = sceneManager;
            _scene = scene;
        }

        public override void Enter()
        {
            _sceneManager.SetScene(_scene);
        }
    }
}