using Codebase.Infrastructure.Factory;
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

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;
        
        private void Update() => 
            SetDestinationForAgent();

        private void SetDestinationForAgent()
        {
            if (HeroTransformInitialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        private bool HeroTransformInitialized() => 
            _heroTransform != null;

        private bool HeroNotReached() =>
            Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
        
    }
}