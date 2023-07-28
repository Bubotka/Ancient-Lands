using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private const string InitialPointTag = "InitialPoint";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, 
      IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
    {
      _stateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
      _uiFactory = uiFactory;
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
      InitUIRoot();
      InitGameWorld();
      InformProgressReaders();

      _stateMachine.Enter<GameLoopState>();
    }

    private void InitUIRoot() => 
      _uiFactory.CreateUIRoot();

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld()
    {
      InitSpawners();
      InitLootPieces();
      GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(InitialPointTag));
      InitHud(hero);
    }

    private void InitSpawners()
    {
      string sceneKey = SceneManager.GetActiveScene().name;
      LevelStaticData levelData = _staticData.ForLevel(sceneKey);
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
      {
        _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
      }
    }

    private void InitLootPieces()
    {
      foreach (string key in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary.Keys)
      {
        LootPiece lootPiece = _gameFactory.CreateLoot();
        lootPiece.GetComponent<UniqueId>().Id = key;
      }
    }

    private void InitHud(GameObject hero)
    {
      GameObject hud = _gameFactory.CreateHud();
      
      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }
  }
}