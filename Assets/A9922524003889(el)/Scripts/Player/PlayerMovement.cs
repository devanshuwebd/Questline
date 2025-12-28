using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float sprintSpeed = 8f;
        public float rotationSmoothTime = 0.12f;
        public float speedChangeRate = 10.0f;

        [Header("Jump & Gravity")]
        public float jumpHeight = 1.2f;
        public float gravity = -15.0f;
        
        [Header("References")]
        public Transform cameraTransform;

        private CharacterController _controller;
        private PlayerInputHandler _input;
        private float _speed;
        private float _verticalVelocity;
        private float _rotationVelocity;

        private void Start() {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputHandler>();
            if (cameraTransform == null && Camera.main != null) 
                cameraTransform = Camera.main.transform;
        }

        private void Update() {
            ApplyGravity();
            Move();
            Jump();
        }

        private void Move() {
            float targetSpeed = _input.IsSprinting ? sprintSpeed : walkSpeed;
            if (_input.MoveInput == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            
            // Speed smoothing
            if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) > 0.1f)
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * _input.MoveInput.magnitude, Time.deltaTime * speedChangeRate);
            else
                _speed = targetSpeed;

            // Rotation & Movement
            if (_input.MoveInput != Vector2.zero) {
                float targetRotation = Mathf.Atan2(_input.MoveInput.x, _input.MoveInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity, rotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
                
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            } else {
                _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
        }

        private void ApplyGravity() {
            if (_controller.isGrounded && _verticalVelocity < 0.0f) 
                _verticalVelocity = -2f;
            else 
                _verticalVelocity += gravity * Time.deltaTime;
        }

        private void Jump() {
            if (_controller.isGrounded && _input.JumpTriggered) 
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}