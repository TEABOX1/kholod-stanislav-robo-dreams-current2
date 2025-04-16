using UnityEngine;
using UnityEngine.AI;

namespace AllInOne
{
    public class ApproachBehaviour : BehaviourStateBase
    {
        private readonly NavMeshAgent _agent;
        private readonly CharacterController _characterController;
        private readonly Transform _characterTransform;

        public ApproachBehaviour(StateMachine stateMachine, byte stateId, EnemyController enemyController)
            : base(stateMachine, stateId, enemyController)
        {
            _agent = enemyController.NavMeshAgent;
            _characterController = enemyController.CharacterController;
            _characterTransform = enemyController.CharacterTransform;
        }

        public override void Enter()
        {
            base.Enter();

            _agent.speed = enemyController.Data.ChaseSpeed;
            _agent.isStopped = false;

            SetDestinationToPlayer();
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (enemyController.Playerdar.CurrentTarget == null)
                return;

            SetDestinationToPlayer();

            Vector3 velocity = _agent.desiredVelocity;
            velocity.y = 0;
            _characterController.Move(velocity * deltaTime + Physics.gravity * deltaTime);

            _agent.nextPosition = _characterTransform.position;

            Vector3 dir = _agent.desiredVelocity;
            dir.y = 0;
            if (dir.magnitude > 0.1f)
            {
                _characterTransform.rotation = Quaternion.LookRotation(dir);
            }
        }

        public override void Exit()
        {
            _agent.isStopped = true;
        }

        public override void Dispose()
        {
        }

        private void SetDestinationToPlayer()
        {
            Vector3 target = enemyController.Playerdar.CurrentTarget.TargetPivot.position;
            _agent.SetDestination(target);
        }
    }
}
