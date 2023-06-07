using Cinemachine;
using Codebase.Hero;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services.PersistentProgress;
using Codebase.Logic;
using Codebase.UI;
using UnityEngine;

namespace Codebase.Infrastructure.States
{
    public class LoadSceneState : IPayloadedState<string>
    {
        private const string Playerinitpoint = "PlayerInitPoint";
        private const string Enemyspawner = "EnemySpawner";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void InitGameWorld()
        {
            InitSpawners();
            
            GameObject initPoint = GameObject.FindWithTag(Playerinitpoint);

            GameObject hero = _gameFactory.CreateHero(initPoint);
            
            _gameFactory.CreateHud(hero);

            SetCameraFollow(hero);
        }

        private void InitSpawners()
        {
            foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(Enemyspawner))
            {
                var spawner = spawnerObject.GetComponent<EnemySpawner>();
                _gameFactory.Register(spawner);
            }
        }

        private static void SetCameraFollow(GameObject hero)
        {
            CinemachineFreeLook cinemachine = Camera.main.GetComponentInChildren<CinemachineFreeLook>();
            cinemachine.m_Follow = hero.transform;
            cinemachine.m_LookAt = hero.transform;
        }
    }
}