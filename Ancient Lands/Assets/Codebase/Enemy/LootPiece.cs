using System;
using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace Codebase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        public GameObject Skull;
        public GameObject PickupFxPrefab;
        public TextMeshPro LootText;
        public GameObject PickupPopup;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }
        
        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) => PickUp();

        private void PickUp()
        {
            if(_picked)
                return;
            
            _picked = true;

            UpdateWorldData();
            HideSword();
            PlayPickupFx();
            ShowText();

            StartCoroutine(StartDestroyTimer());
            
            Debug.Log(_worldData.LootData.Collected);
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSword() => 
            Skull.SetActive(false);

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            
            Destroy(gameObject);
        }

        private void PlayPickupFx() => 
            Instantiate(PickupFxPrefab, transform.position+transform.up, Quaternion.identity);

        private void ShowText()
        {
            LootText.text = $"{_loot.Value}";
            PickupPopup.SetActive(true);
        }
    }
}