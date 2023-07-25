using UnityEngine;

namespace CodeBase.Services.Input
{
    public class InputService : IInputService
    {
        private PlayerInput _playerInput;

        public InputService()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
        }

        public Vector2 Move 
            => _playerInput.Player.Move.ReadValue<Vector2>();
        
        public Vector2 Look 
            => _playerInput.Player.Look.ReadValue<Vector2>();

        public bool IsAttackButtonUp() => 
            _playerInput.Player.Attack.IsPressed();
    }
}
