using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService:IService
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        bool IsAttackButtonUp();
    }
}