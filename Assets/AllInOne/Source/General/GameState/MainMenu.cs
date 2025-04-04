
namespace AllInOne
{
    public class MainMenu : GameStateBase
    {
        public MainMenu(StateMachine stateMachine, byte stateId, ISceneManager sceneManager, Scenes scene) : base(stateMachine, stateId, sceneManager, scene)
        {
        }

        public override void Dispose()
        {
        }
    }
}