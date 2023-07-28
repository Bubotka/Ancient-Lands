using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class WindowBase:MonoBehaviour
    {
        public Button CloseButton;
        
        protected IPersistentProgressService progressService;
        protected PlayerProgress progress=>progressService.Progress;

        public void Construct(IPersistentProgressService progressService) => 
            this.progressService = progressService;

        private void Awake() => 
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            CleanUp();

        protected virtual void OnAwake() => 
            CloseButton.onClick.AddListener(()=>Destroy(gameObject));

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
            
        }

        protected virtual void CleanUp()
        {
        }
        
    }
}