using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class AttackBehaviour : BehaviourStateBase
    {
        public enum State
        {
            PreparingAttack,
            Hitting,
            Resting,
        }

        private readonly Transform _characterTransform;

        private State _state;
        private float _time;

        public AttackBehaviour(StateMachine stateMachine, byte stateId, EnemyController enemyController) : base(stateMachine, stateId, enemyController)
        {
            _characterTransform = enemyController.CharacterTransform;
        }

        public override void Enter()
        {
            base.Enter();
            _time = 0f;
            _state = State.Resting;

            conditions = new List<IStateCondition>
                { new BaseCondition((byte)EnemyBehaviour.Shoot, RangeChanged) };
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            //if (_state != State.Hitting)
            MoveCloser();

            switch (_state)
            {
                case State.PreparingAttack:
                    PreparingAttackUpdate(deltaTime);
                    break;
                case State.Hitting:
                    HittingUpdate(deltaTime);
                    break;
                case State.Resting:
                    RestingUpdate(deltaTime);
                    break;
            }
        }

        private void PreparingAttackUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < 0.1)
                return;

            Hit();
        }

        private void HittingUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < 0.1)
                return;

            //if (!RangeChanged())
            //{
                enemyController.HitScanGun.Hit(enemyController.Playerdar.PlayerCollider);
            //}

            _time = 0f;
            _state = State.Resting;
        }
        private void RestingUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < 0.5)
                return;

            _time = 0f;
            _state = State.PreparingAttack;
        }

        private void Hit()
        {
            //RangeChanged();

            _time = 0f;
            _state = State.Hitting;
        }

        protected void MoveCloser()
        {
            Vector3 direction = Vector3.ProjectOnPlane(enemyController.Playerdar.CurrentTarget.TargetPivot.position
                                                       - _characterTransform.position, Vector3.up).normalized;

            float distance = Vector3.Distance(enemyController.Playerdar.CurrentTarget.TargetPivot.position, _characterTransform.position);

            if (distance > 3 || distance < 3)
            {
                _characterTransform.position = Vector3.Lerp(_characterTransform.position,
                                              enemyController.Playerdar.CurrentTarget.TargetPivot.position - direction * 2f, Time.deltaTime * enemyController.Data.ChaseSpeed);
            }
            _characterTransform.rotation = Quaternion.LookRotation(direction);
        }

        private bool RangeChanged()
        {
            Vector3 position = _characterTransform.position;
            Vector3 playerPosition = enemyController.Playerdar.CurrentTarget.TargetPivot.position;

            Vector3 playerDirection = Vector3.ProjectOnPlane(playerPosition - position, Vector3.up);

            return playerDirection.sqrMagnitude > 3 * 3;
        }

        public override void Dispose()
        {
        }


    }
}