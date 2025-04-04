using UnityEngine;
using UnityEngine.UI;

namespace AllInOne
{
    public class GameplayMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        [SerializeField] private Button _confrimButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private InputController _inputController;

        public bool Enabled
        {
            get => _canvas.enabled;
            set
            {
                if (_canvas.enabled == value)
                    return;
                _canvas.enabled = value;
                ServiceLocator.Instance.GetService<IGameStateProvider>()?.SetGameState(value ? GameState.Paused : GameState.Gameplay);
            }
        }
        
        private void Awake()
        {
            _confrimButton.onClick.AddListener(ConfirmButtonHandler);
            _cancelButton.onClick.AddListener(CancelButtonHandler);
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService<InputController>().OnEscape += EscapeHandler;
        }

        private void EscapeHandler()
        {
            Enabled = !Enabled;
        }

        private void ConfirmButtonHandler()
        {
            Debug.Log("here");
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }

        private void CancelButtonHandler()
        {
            Enabled = false;
        }
    }
}