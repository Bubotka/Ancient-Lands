namespace Codebase.Infrastructure.States
{
    public interface IState:IExitableState
    {
        void Enter();
    }

    public interface IExitableState
    {
        void Exit();
    }
    
    public interface IPayloadedState<TPayloaded>:IExitableState
    {
        void Enter(TPayloaded payload);
    }
}