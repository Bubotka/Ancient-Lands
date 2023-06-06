using System;
using CodeBase.Data;
using Codebase.Infrastructure.Services;
using Codebase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using Codebase.Logic;
using UnityEngine;

namespace Codebase.Hero
{
    [RequireComponent(typeof(HeroAnimator),typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour,ISavedProgressReader
    {
        public HeroAnimator HeroAnimator;
        public CharacterController CharacterController;
        
        private IInputService _input;
        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private Stats _stats;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if(_input.Player.Attack.triggered&&!HeroAnimator.IsAttacking)
                HeroAnimator.PlayAttack();
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.Stats;

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius , _hits, _layerMask);

        private Vector3 StartPoint()=>
            new Vector3(transform.position.x,CharacterController.center.y/2,transform.position.z);
    }
}