using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.Ads;
using CodeBase.Services.IAP;
using CodeBase.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem AdItem;
        public ShopItemsContainer ShopItemsContainer;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService, IIAPService iapService,
            IAssetProvider assets)
        {
            base.Construct(progressService);
            AdItem.Construct(adsService,progressService);
            ShopItemsContainer.Construct(iapService, progressService, assets);
        }
         
        protected override void Initialize()
        {
            AdItem.Initialize();
            ShopItemsContainer.Initialize();
            RefreshSkullText();
        }

        protected override void SubscribeUpdates()
        {
            AdItem.Subscribe();
            ShopItemsContainer.Subscribe();
            progress.WorldData.LootData.Changed += RefreshSkullText;
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            AdItem.CleanUp();
            ShopItemsContainer.CleanUp();
            progress.WorldData.LootData.Changed -= RefreshSkullText;
        }

        private void RefreshSkullText() => 
            SkullText.text = progress.WorldData.LootData.Collected.ToString();
    }
} 