using System;
using UnityEngine;

namespace Codebase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange:MonoBehaviour
    {
        public Attack Attack;
        public TriggerObserver TriggerObserver;

        private void Start()
        {
            TriggerObserver.TriggerEnter+=TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;
        }

        private void TriggerEnter(Collider obj) => 
            Attack.EnableAttack();

        private void TriggerExit(Collider obj) => 
            Attack.DisableAttack();
    }
}