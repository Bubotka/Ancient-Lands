﻿using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using Codebase.Infrastructure.Services.SaveLaod;

namespace Codebase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadSerivce;

        public LoadProgressState(GameStateMachine stateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadSerivce)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadSerivce = saveLoadSerivce;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _stateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);

        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _progressService.Progress = _saveLoadSerivce.LoadProgress() ?? NewProgress();
        }


        private PlayerProgress NewProgress()
        {
            PlayerProgress progress = new PlayerProgress(initialLevel: "Main");

            progress.HeroState.MaxHP = 100;
            progress.HeroState.ResetHP();
            progress.HeroStats.Damage=25;
            progress.HeroStats.DamageRadius=0.5f;
            
            return progress;
        }
    }
}