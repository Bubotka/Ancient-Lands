﻿using System;
using System.Linq;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services;
using UnityEngine;

namespace Codebase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;
        public float AttackCooldown;
        public float Cleavage = 0.5f;
        public float EffectiveDistance= 0.5f;

        private IGameFactory _factory;
        private Transform _heroTransform;
        private float _attackCooldown;
        private bool _isAttacking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];

        private void Awake()
        {
            _factory = AllServices.Container.Single<IGameFactory>();
            _factory.HeroCreated += OnHeroCreated;
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                
            }
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();
            
            return hitsCount > 0;
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z)+transform.forward*EffectiveDistance;


        private void OnAttackEnded() 
        {
            _attackCooldown = AttackCooldown;
            _isAttacking = false;
        }
        
        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            Animator.PlayAttack();

            _isAttacking = true;
        }

        private bool CanAttack() => 
            !_isAttacking&&CooldownIsUp();

        private bool CooldownIsUp() => 
            _attackCooldown <= 0;

        private void OnHeroCreated() => 
            _heroTransform = _factory.HeroGameObject.transform;
    }
}