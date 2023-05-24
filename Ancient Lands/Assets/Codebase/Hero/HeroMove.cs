using Codebase.Data;
using Codebase.Infrastructure;
using Codebase.Infrastructure.Input;
using Codebase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebase.Hero
{
    public class HeroMove : MonoBehaviour,ISavedProgress
    {
        public CharacterController _characterController;
        public float MoveSpeed = 2;
        public float TurnSmoothTime = 0.2f;

        private float _turnVelocity;
        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void OnEnable() =>
            _inputService.Player.Enable();

        private void OnDisable() =>
            _inputService.Player.Disable();

        private void Update()
        {
            Vector3 moveDir = Vector3.zero;

            if (_inputService.Player.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                Vector3 direction = ReadMoveValue().normalized;
                Debug.Log(direction);

                float targetAngle = CalculateRotateAngle(direction);
                float angle = SmoothAngle(targetAngle);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            moveDir += Physics.gravity;

            _characterController.Move(moveDir.normalized * MoveSpeed * Time.deltaTime);
        }

        private Vector3 ReadMoveValue() =>
            new Vector3(GetValueX(), 0, GetValueZ());

        private float SmoothAngle(float targetAngle) =>
            Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnVelocity, TurnSmoothTime);

        private float CalculateRotateAngle(Vector3 directionNormalized) =>
            Mathf.Atan2(directionNormalized.x, directionNormalized.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;

        private float GetValueZ() =>
            _inputService.Player.Move.ReadValue<Vector2>().y;

        private float GetValueX() =>
            _inputService.Player.Move.ReadValue<Vector2>().x;

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (CurrentLevel()==playerProgress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = playerProgress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null) 
                    Warp(to: savedPosition);
            }
        }

        private static string CurrentLevel() => 
            SceneManager.GetActiveScene().name;

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(),
                transform.position.AsVectorData());
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector();
            _characterController.enabled = true;
        }
    }
}