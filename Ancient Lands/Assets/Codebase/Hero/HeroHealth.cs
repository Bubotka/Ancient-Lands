using System;
using CodeBase.Data;
using Codebase.Infrastructure.Services.PersistentProgress;
using Codebase.Logic;
using UnityEngine;

namespace Codebase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth ,ISavedProgress
    {
        public HeroAnimator Animator;

        public event Action HealthChanged;

        private State _state;

        public float Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        public float Current
        {
            get => _state.CurrentHP;
            set
            {
                if (_state.CurrentHP != value)
                {
                    _state.CurrentHP = value;
                    HealthChanged?.Invoke();
                }
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;
            Animator.PlayHit();
            Current -= damage;
        }
    }
}