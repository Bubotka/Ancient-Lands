namespace Codebase.Infrastructure.AssetManagement
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
}