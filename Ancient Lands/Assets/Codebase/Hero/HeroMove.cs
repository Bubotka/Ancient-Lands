using System;
using Codebase.Infrastructure;
using UnityEngine;

namespace Codebase.Hero
{
    public class HeroMove : MonoBehaviour
    {
        public CharacterController _CharacterController;
        public float MoveSpeed = 2;

        private InputService _inputService;

        private void Awake()
        {
            _inputService = Game.InputService;
        }

        private void OnEnable() =>
            _inputService.Player.Enable();

        private void OnDisable() =>
            _inputService.Player.Disable();

        private void Update()
        {
            Vector3 movementVector = new Vector3(GetValueX(), 0, GetValueZ());
            ;
            Vector3 movementVectorNormalized = movementVector.normalized;

            transform.forward = movementVectorNormalized;

            movementVectorNormalized += Physics.gravity;

            _CharacterController.Move(movementVectorNormalized * MoveSpeed * Time.deltaTime);
        }

        private float GetValueZ() =>
            _inputService.Player.Move.ReadValue<Vector2>().y;

        private float GetValueX() =>
            _inputService.Player.Move.ReadValue<Vector2>().x;
    }
}