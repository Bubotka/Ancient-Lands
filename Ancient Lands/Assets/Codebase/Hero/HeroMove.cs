using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _turnSmoothTime=0.2f;

    private float _turnSmoothVelocity;
    private IInputService _inputService;
    private Camera _camera;


    private void Awake() => 
      _inputService = AllServices.Container.Single<IInputService>();

    private void Start() =>
      _camera = Camera.main;

    private void Update()
    {
      Vector3 movementVector = Vector3.zero;

      if (_inputService.Move.SqrMagnitude() > 0.1)
      {
        Vector3 direction = new Vector3(_inputService.Move.x,0,_inputService.Move.y);

        float targetAngle = GetRotateAngle(direction);
        float angle = SmoothAngle(targetAngle);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        movementVector = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      }

      movementVector += Physics.gravity;
            
      _characterController.Move(movementVector * Time.deltaTime);
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

      Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
      if (savedPosition != null) 
        Warp(to: savedPosition);
    }
    
    private float SmoothAngle(in float targetAngle) => 
      Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

    private float GetRotateAngle(Vector3 direction) => 
      Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;

    private void Warp(Vector3Data to)
    {
      _characterController.enabled = false;
      transform.position = to.AsUnityVector().AddY(_characterController.height);
      _characterController.enabled = true;
    }
  }
}