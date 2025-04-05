using System;
using UnityEngine;

namespace AllInOne
{
    public class DefendFlagMode : MonoServiceBase, IKillThemAllModeService, IModeService
    {
        public event Action<bool> OnComplete;

        [SerializeField] private RoomController[] _hordeSpawners;
        [SerializeField] private float _duration;
        [SerializeField] private ScoreSystem _scoreSystem;

        private float _reciprocal;

        public override Type Type { get; } = typeof(IModeService);
        public Vector3 MapCentre => transform.position;

        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Instance.AddServiceExplicit(typeof(IKillThemAllModeService), this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ServiceLocator.Instance.RemoveServiceExplicit(typeof(IKillThemAllModeService), this);
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService<IPlayerService>().Player.Health.OnDeath += PlayerDeathHandler;
            enabled = false;
            Begin();
        }

        public void Begin()
        {
            _reciprocal = 1f / _duration;

            for (int i = 0; i < _hordeSpawners.Length; ++i)
                _hordeSpawners[i].enabled = true;

            enabled = true;
        }

        private void Update()
        {
            if (_scoreSystem.KDA.x >= 4)
            {
                End();
                return;
            }
        }

        private void End()
        {
            enabled = false;

            for (int i = 0; i < _hordeSpawners.Length; ++i)
            {
                RoomController spawner = _hordeSpawners[i];
                spawner.enabled = false;
                spawner.OnEnemyDeath += SpawnerEnemyDeathHandler;
            }
        }

        private void SpawnerEnemyDeathHandler()
        {
            if (_scoreSystem.KDA.x >= 5)
            {
                InputController inputController =
                    ServiceLocator.Instance.GetService<InputController>();
                inputController.enabled = false;
                inputController.DisableEscape();
                OnComplete?.Invoke(true);
            }
        }

        private void PlayerDeathHandler()
        {
            enabled = false;
            ServiceLocator.Instance.GetService<InputController>().DisableEscape();
            OnComplete?.Invoke(false);
        }
    }
}