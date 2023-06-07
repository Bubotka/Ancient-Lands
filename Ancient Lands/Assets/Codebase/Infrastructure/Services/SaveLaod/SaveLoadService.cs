using System.IO;
using System.Text;
using CodeBase.Data;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Codebase.Infrastructure.Services.SaveLaod
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            string filePath = Application.persistentDataPath + "/SaveData.json";

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.Write(_progressService.Progress.ToJson());
            }
        }

        public PlayerProgress LoadProgress()
        {
            string filePath = Application.persistentDataPath + "/SaveData.json";

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    PlayerProgress data = reader.ReadToEnd().ToDeserialized<PlayerProgress>();
                    return data;
                }
            }

            return null;
        }
    }
}