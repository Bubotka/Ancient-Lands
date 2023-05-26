using System;
using UnityEngine;
using UnityEngine.AI;

namespace Codebase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public EnemyAnimator Animator;
        private float MinimalVelocity = 0.1f;

        private void Update()
        {
            if (ShouldMove())
            {
                Animator.Move(Agent.velocity.magnitude);
            }
            else
            {
                Animator.StopMove();
            }
        }

        private bool ShouldMove() => 
            Agent.velocity.magnitude > MinimalVelocity && Agent.remainingDistance > Agent.radius;
    }
}