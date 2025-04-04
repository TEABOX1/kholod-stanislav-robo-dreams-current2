using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace AllInOne
{
    public class PlayerService : MonoServiceBase, IPlayerService
    {
        [SerializeField] private PlayerController _playerController;

        public override Type Type { get; } = typeof(IPlayerService);
        
        public PlayerController Player => _playerController;

        private Dictionary<Collider, PlayerController> _playerControllers = new();
        
        protected override void Awake()
        {
            base.Awake();
            _playerControllers.Add(_playerController.CharacterController, _playerController);
        }

        public bool IsPlayer(Collider collider)
        {
            return _playerControllers.ContainsKey(collider);
        }
    }
}