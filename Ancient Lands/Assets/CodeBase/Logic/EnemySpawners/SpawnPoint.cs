﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
  public class SpawnPoint : MonoBehaviour, ISavedProgress
  {
    public MonsterTypeId MonsterTypeId;

    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    public string Id { get; set; }
    private bool _slain;

    public void Construct(IGameFactory factory) => 
      _factory = factory;

    private void OnDestroy()
    {
      if (_enemyDeath != null)
        _enemyDeath.Happened -= Slay;
    }


    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(Id))
        _slain = true;
      else
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      List<string> slainSpawnersList = progress.KillData.ClearedSpawners;
      
      if(_slain && !slainSpawnersList.Contains(Id))
        slainSpawnersList.Add(Id);
    }

    private async void Spawn()
    {
      GameObject monster = await _factory.CreateMonster(MonsterTypeId, transform);
      _enemyDeath = monster.GetComponent<EnemyDeath>();
      _enemyDeath.Happened += Slay;
    }

    private void Slay()
    {
      if (_enemyDeath != null)
        _enemyDeath.Happened -= Slay;
      
      _slain = true;
    }
  }
}