using System;
using Cinemachine;
using Codebase.Infrastructure;
using Codebase.Infrastructure.AssetManagement;
using UnityEngine;

namespace Codebase.Hero
{
    public class CameraLookRotation : MonoBehaviour
    {
        public float MouseSensivityX = 2;
        public float MouseSensivityY = 1;

        private InputService _inputService;
        private CinemachineFreeLook _cineMachine;

        private void Awake()
        {
            _inputService = Game.InputService;
            _cineMachine = GetComponent<CinemachineFreeLook>();
        }

        private void OnEnable() =>
            _inputService.Player.Enable();

        private void OnDisable() =>
            _inputService.Player.Disable();

        private void LateUpdate()
        {
            Vector2 delta = ReadLookValue();
            _cineMachine.m_XAxis.Value += delta.x * MouseSensivityX * 10 * Time.deltaTime;
            _cineMachine.m_YAxis.Value -= delta.y * MouseSensivityY * Time.deltaTime;
        }

        private Vector2 ReadLookValue() =>
            _inputService.Player.Look.ReadValue<Vector2>();
    }
}