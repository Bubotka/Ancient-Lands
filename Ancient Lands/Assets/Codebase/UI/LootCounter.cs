using CodeBase.Data;
using Codebase.Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace Codebase.UI
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.LootData.Changed += UpdateCounter;
        }

        private void Start()
        {
            UpdateCounter(); 
        }

        private void UpdateCounter()
        {
            Counter.text = $"{_worldData.LootData.Collected}";
        }


    }
}