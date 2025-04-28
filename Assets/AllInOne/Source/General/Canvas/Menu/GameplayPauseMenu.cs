using UnityEngine;
using UnityEngine.UI;

namespace AllInOne
{
    public class GameplayPauseMenu : MonoBehaviour
    {
        public enum MenuState
        {
            Hidden,
            Menu,
            Settings,
            Exit
        }
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Canvas _exitCanvas;
        [SerializeField] private SoundMenu _soundMenu;
        
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _confrimButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _closeSettingsButton;
        [SerializeField] private Button _closeMenuButton;

        private InputController _inputController;

        private MenuState _menuState = MenuState.Hidden;
        private ISaveService _saveService;

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
            _exitButton.onClick.AddListener(ExitButtonHandler);
            _settingsButton.onClick.AddListener(SettingsButtonHandler);
            _closeSettingsButton.onClick.AddListener(CloseSettingsButtonHandler);
            _closeMenuButton.onClick.AddListener(CloseMenuButtonHandler);
        }

        private void Start()
        {
            _canvas.enabled = false;
            _exitCanvas.enabled = false;
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();
            _inputController.OnEscape += EscapeHandler;
        }

        private void EscapeHandler()
        {
            switch (_menuState)
            {
                case MenuState.Hidden:
                    Enabled = true;
                    _menuState = MenuState.Menu;
                    break;
                case MenuState.Menu:
                    Enabled = false;
                    _menuState = MenuState.Hidden;
                    break;
                case MenuState.Settings:
                    CloseSettingsButtonHandler();
                    _menuState = MenuState.Menu;
                    break;
                case MenuState.Exit:
                    CancelButtonHandler();
                    _menuState = MenuState.Menu;
                    break;
            }
        }

        private void ExitButtonHandler()
        {
            _exitCanvas.enabled = true;
            _canvas.enabled = false;
            _menuState = MenuState.Exit;
        }

        private void SettingsButtonHandler()
        {
            _canvas.enabled = false;
            _soundMenu.Enabled = true;
            _menuState = MenuState.Settings;
        }

        private void CloseSettingsButtonHandler()
        {
            _canvas.enabled = true;
            _soundMenu.Enabled = false;
            _menuState = MenuState.Menu;
            _saveService.SaveAll();
        }
        
        private void CloseMenuButtonHandler()
        {
            Enabled = false;
            _menuState = MenuState.Hidden;
        }
        
        private void ConfirmButtonHandler()
        {
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }

        private void CancelButtonHandler()
        {
            _exitCanvas.enabled = false;
            _canvas.enabled = true;
            _menuState = MenuState.Menu;
        }
    }
}