using UnityEngine;
using UnityEngine.InputSystem;

namespace Isekaid
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private Camera viewCamera;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float sprintSpeed = 5.5f;
        [SerializeField] private float mouseSensitivity = 0.085f;

        private CharacterController controller;
        private InputActionAsset runtimeActions;
        private InputAction move;
        private InputAction look;
        private InputAction sprint;
        private InputAction jump;
        private InputAction interact;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float pitch;
        private float verticalSpeed;

        public Camera ViewCamera => viewCamera;
        public bool InputEnabled { get; set; }
        public bool InteractPressed => interact != null && interact.WasPerformedThisFrame();

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            runtimeActions = Instantiate(inputActions);
            move = runtimeActions.FindAction("Player/Move", true);
            look = runtimeActions.FindAction("Player/Look", true);
            sprint = runtimeActions.FindAction("Player/Sprint", true);
            jump = runtimeActions.FindAction("Player/Jump", true);
            interact = runtimeActions.FindAction("Player/Interact", true);
        }

        private void OnEnable() => runtimeActions.FindActionMap("Player", true).Enable();
        private void OnDisable() => runtimeActions?.Disable();

        private void OnDestroy()
        {
            if (runtimeActions != null)
                Destroy(runtimeActions);
        }

        private void Update()
        {
            if (!InputEnabled || Time.timeScale == 0f)
                return;

            var delta = look.ReadValue<Vector2>();
            float sensitivity = look.activeControl?.device is Gamepad ? 120f * Time.deltaTime : mouseSensitivity;
            transform.Rotate(0f, delta.x * sensitivity, 0f);
            pitch = Mathf.Clamp(pitch - delta.y * sensitivity, -80f, 80f);
            viewCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            var movement = Vector2.ClampMagnitude(move.ReadValue<Vector2>(), 1f);
            var velocity = (transform.right * movement.x + transform.forward * movement.y) *
                (sprint.IsPressed() ? sprintSpeed : walkSpeed);
            if (controller.isGrounded)
            {
                verticalSpeed = -2f;
                if (jump.WasPressedThisFrame())
                    verticalSpeed = 4.5f;
            }
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
            velocity.y = verticalSpeed;
            controller.Move(velocity * Time.deltaTime);
        }

        public void ResetPosition()
        {
            controller.enabled = false;
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            pitch = 0f;
            verticalSpeed = 0f;
            viewCamera.transform.localRotation = Quaternion.identity;
            controller.enabled = true;
        }
    }
}
