using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AllInOne
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _exitButton;
        [SerializeField] private float _delay;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _buttonGroup;
        [SerializeField] private bool _showOnWin;

        private void Awake()
        {
            _canvas.enabled = false;
            _exitButton.onClick.AddListener(ExitButtonHandler);
            ServiceLocator.Instance.GetService<IModeService>().OnComplete += ModeCompletedHandler;
        }

        public void Show()
        {
            _canvas.enabled = true;
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _buttonGroup.alpha = 0f;
            _buttonGroup.interactable = false;

            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            YieldInstruction delay = new WaitForSeconds(_delay);
            
            yield return delay;

            float time = 0f;
            float reciprocal = 1f / _fadeDuration;

            while (time < _fadeDuration)
            {
                _canvasGroup.alpha = time * reciprocal;
                yield return null;
                time += Time.deltaTime;
            }
            _canvasGroup.alpha = 1f;
            
            yield return delay;
            
            time = 0f;
            while (time < _fadeDuration)
            {
                _buttonGroup.alpha = time * reciprocal;
                yield return null;
                time += Time.deltaTime;
            }
            _buttonGroup.alpha = 1f;
            
            _canvasGroup.interactable = true;
            _buttonGroup.interactable = true;
        }
        
        private void ModeCompletedHandler(bool success)
        {
            if (success != _showOnWin)
                return;
            Show();
        }
        
        private void ExitButtonHandler()
        {
            ServiceLocator.Instance.GetService<IGameStateProvider>().SetGameState(GameState.MainMenu);
        }
    }
}