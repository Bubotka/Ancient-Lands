using System;
using System.Collections.Generic;
using Codebase.Hero;
using Codebase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
    public interface IGameFactory:IService
    {
        GameObject CreateHero(GameObject at);
        void CreateHud(GameObject hero);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        
        GameObject HeroGameObject { get; set; }

        event Action HeroCreated;
        
        void Cleanup();
    }
}