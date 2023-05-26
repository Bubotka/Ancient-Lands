using System;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Codebase.Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        public NavMeshAgent Agent;
        private Transform _heroTransform;
        private IGameFactory _gameFactory;
        private float MinimalDistance = 1f;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if(_gameFactory.HeroGameObject!=null)
                InitializeHeroTransform();
            else
                _gameFactory.HeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            if (HeroTransformInitialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        private bool HeroTransformInitialized() => 
            _heroTransform != null;

        private bool HeroNotReached() =>
            Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;
    }
}