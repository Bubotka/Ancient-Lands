using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        
        private Transform _uiRoot;

        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _staticData = staticData;
            _progressService = progressService;
            _assets = assets;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            WindowBase window = Object.Instantiate(config.Prefab, _uiRoot);
            window.Construct(_progressService);
        }

        public void CreateUIRoot() => 
            _uiRoot= _assets.Instantiate(AssetPath.UIRootPath).transform;
    }
}