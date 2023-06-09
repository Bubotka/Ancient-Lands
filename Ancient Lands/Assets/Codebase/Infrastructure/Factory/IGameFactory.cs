using System.Collections.Generic;
using Codebase.Enemy;
using Codebase.Hero;
using Codebase.Infrastructure.Services;
using Codebase.Infrastructure.Services.PersistentProgress;
using Codebase.StaticData;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
    public interface IGameFactory:IService
    {
        GameObject CreateHero(GameObject at);
        void CreateHud(GameObject hero);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        void Cleanup();

        void Register(ISavedProgressReader progressReader);
        GameObject CreateMonster(MonsterTypeID typeID, Transform parent);
        LootPiece CreateLoot();
    }
}