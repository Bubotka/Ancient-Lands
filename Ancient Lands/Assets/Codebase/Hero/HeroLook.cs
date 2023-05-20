using System;
using Cinemachine;
using UnityEngine;

namespace Codebase.Hero
{
    public class HeroLook: MonoBehaviour
    {
        private CinemachineFreeLook _cinemachineCamera;

        private void Awake()
        {
            _cinemachineCamera = GetComponent<CinemachineFreeLook>();
        }
        
        
    }
}