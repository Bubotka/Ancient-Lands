using System;
using Cinemachine;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroLook : MonoBehaviour
    {
        [Range(0, 1)] public float SensetivityX = 1;
        [Range(0, 1)] public float SensetivityY = 1;

        private CinemachineFreeLook _camera;
        private IInputService _inputService;


        private void Start()
        {
            _camera = Camera.main.GetComponentInChildren<CinemachineFreeLook>();
            _camera.Follow = transform;
            _camera.LookAt = transform;
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Update()
        {
            if (_inputService.Look.SqrMagnitude() > 0) RotateCamera();
        }

        private void RotateCamera()
        {
            _camera.m_XAxis.Value += _inputService.Look.x * SensetivityX;
            _camera.m_YAxis.Value -= _inputService.Look.y * SensetivityY / 100;
        }
    }
}