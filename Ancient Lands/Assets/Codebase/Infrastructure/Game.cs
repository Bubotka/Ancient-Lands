using Codebase.Infrastructure.Services;
using Codebase.Infrastructure.States;
using Codebase.Logic;

namespace Codebase.Infrastructure
{
    public  class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
        }
    }
}