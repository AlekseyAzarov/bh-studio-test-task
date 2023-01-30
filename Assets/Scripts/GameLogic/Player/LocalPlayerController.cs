using Mirror;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(AbstractPlayerCamera), typeof(PlayerController))]
    public class LocalPlayerController : NetworkBehaviour
    {
        private AbstractPlayerCamera _camera;
        private PlayerController _playerController;
        private float _sensivity;
        private IInputService _inputService;

        private void Start()
        {
            _camera = GetComponent<AbstractPlayerCamera>();
            _playerController = GetComponent<PlayerController>();
        }

        public void Init(IInputService inputService, float sensivity)
        {
            _inputService = inputService;
            _sensivity = sensivity;

            _inputService.SubscribeToHorizontalAndVerticalAxis(AxisType.Movement, OnMoveAxis);
            _inputService.SubscribeToHorizontalAndVerticalAxis(AxisType.Look, OnLookAxis);
            _inputService.SubscribeToKey(KeyCode.Mouse0, OnDashButtonDown);
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            _camera.UpdateTransform();
        }

        private void OnDestroy()
        {
            if (!isLocalPlayer) return;

            _inputService.UnsubscribeToHorizontalAndVerticalAxis(AxisType.Movement, OnMoveAxis);
            _inputService.UnsubscribeToHorizontalAndVerticalAxis(AxisType.Look, OnLookAxis);
            _inputService.UnsubscribeToKey(KeyCode.Mouse0, OnDashButtonDown);
        }

        private void OnLookAxis(float horizontal, float vertical)
        {
            _camera.UpdateRotation(vertical * -_sensivity, horizontal * _sensivity);
        }

        private void OnMoveAxis(float horizontal, float vertical)
        {
            var moveDirection = new Vector3(horizontal, 0f, vertical);
            _playerController.Move(moveDirection, _camera.CameraYRotation);
        }

        private void OnDashButtonDown()
        {
            _playerController.Dash();
        }
    }
}
