using System;
using Codebase.Logic;
using UnityEngine;

namespace Codebase.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        public CharacterController _characterController;
        public Animator _animator;

        private static readonly int RunHash = Animator.StringToHash("Run");
        private static readonly int AttackHash = Animator.StringToHash("Attack01");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _attackStateHash = Animator.StringToHash("Attack");
        private readonly int _deathStateHash = Animator.StringToHash("Hit");
        private readonly int _runStateHash = Animator.StringToHash("Die");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        public bool IsAttacking => State == AnimatorState.Attack;

        private void Update() => 
            _animator.SetFloat(RunHash, _characterController.velocity.magnitude,0.1f,Time.deltaTime);

        public void PlayAttack() => 
            _animator.SetTrigger(AttackHash);
        
        public void PlayHit() => 
            _animator.SetTrigger(HitHash);
        
        public void PlayDeath() => 
            _animator.SetTrigger(DieHash);
        
        public void ResetToIdle() => 
            _animator.Play(_idleStateHash);

        public void EnterState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitState(int stateHash) => 
            StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _runStateHash)
                state = AnimatorState.Run;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}