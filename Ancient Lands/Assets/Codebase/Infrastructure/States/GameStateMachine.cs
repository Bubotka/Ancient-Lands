using System;
using System.Collections.Generic;
using Codebase.Infrastructure.Factory;
using Codebase.Infrastructure.Services;
using Codebase.Infrastructure.Services.PersistentProgress;
using Codebase.Infrastructure.Services.SaveLaod;
using Codebase.Logic;

namespace Codebase.Infrastructure.States
{
    public class GameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this,sceneLoader, services),
                [typeof(LoadProgressState)] = new LoadProgressState(this,services.Single<IPersistentProgressService>(),services.Single<ISaveLoadService>()),
                [typeof(LoadSceneState)] = new LoadSceneState(this,sceneLoader,curtain,services.Single<IGameFactory>(),services.Single<IPersistentProgressService>()),
                [typeof(GameLoopState)] = new GameLoopState(this,sceneLoader)
            };
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }
    }
}