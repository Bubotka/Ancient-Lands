using System;
using CodeBase.Data;
using Codebase.Enemy;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services;
using Codebase.Infrastructure.Services.PersistentProgress;
using Codebase.StaticData;
using UnityEngine;

namespace Codebase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeID MonsterTypeID;
        public bool Slain => _slain;
        
        private bool _slain;
        private string _id;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                _slain = true;
            else
                Spawn();
        }

        private void Spawn()
        {
            GameObject monster = _factory.CreateMonster(MonsterTypeID, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null) 
                _enemyDeath.Happened -= Slay;
            
            _slain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if(_slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }
    }
}