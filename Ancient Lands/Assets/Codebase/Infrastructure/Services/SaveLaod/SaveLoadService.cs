using System.IO;
using System.Text;
using Codebase.Data;
using Codebase.Hero;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services.PersistentProgress;
using Unity.VisualScripting;
using UnityEngine;

namespace Codebase.Infrastructure.Services.SaveLaod
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }
        
        public void SaveProgress()
        {
            foreach(ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            string path = Path.Combine(Application.dataPath, "playerData.json");
            
            using (StreamWriter streamWriter = new StreamWriter(path,false,Encoding.UTF8))
            {
                streamWriter.Write(_progressService.Progress.ToJson());
            }
        }

        public PlayerProgress LoadProgress()
        {
            string path = Path.Combine(Application.dataPath, "playerData.json");
                
            if (File.Exists(path))
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string jsonData = streamReader.ReadToEnd();
                    return jsonData.ToDeserialized<PlayerProgress>();
                }
            }

            return null;
        }
    }
}