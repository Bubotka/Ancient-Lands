using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadProgress)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress = 
        _saveLoadProgress.LoadProgress() 
        ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
      var progress =  new PlayerProgress(initialLevel: "Main");

      progress.HeroState.MaxHP = 100;
      progress.HeroStats.Damage = 3;
      progress.HeroStats.DamageRadius = 4f;
      progress.HeroState.ResetHP();

      return progress;
    }
  }
}