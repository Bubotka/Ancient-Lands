using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.IAP;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopItemsContainer : MonoBehaviour
    {
        private const string ShopItemPath = "ShopItem";
        
        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;
        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssetProvider _assets; 
        private List<GameObject> _shopItems = new List<GameObject>();

        public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssetProvider assets)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assets = assets;
        }

        public void Initialize() => 
            RefreshAvailavleItems();

        public void Subscribe()
        {
            _iapService.Initialized += RefreshAvailavleItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailavleItems;
            
        }

        public void CleanUp()
        {
            _iapService.Initialized -= RefreshAvailavleItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailavleItems;
        }

        private async void RefreshAvailavleItems()
        {
            UpdateShopUnavailableObjects();

            if(!_iapService.IsInitialized)
                return;

            ClearShopItems(); 

            await FillShopItems();
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems)
            {
                Destroy(shopItem.gameObject);
            }
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject ShopItemObject = await _assets.Instantiate(ShopItemPath, Parent);
                ShopItem shopItem = ShopItemObject.GetComponent<ShopItem>();

                shopItem.Construct(_iapService, _assets, productDescription);
                shopItem.Initialize();

                _shopItems.Add(ShopItemObject);
            }
        }

        private void UpdateShopUnavailableObjects()
        {
            foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
                shopUnavailableObject.SetActive(!_iapService.IsInitialized);
        }
    }
}