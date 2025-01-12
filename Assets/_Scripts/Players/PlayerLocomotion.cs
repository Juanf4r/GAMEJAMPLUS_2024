using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerLocomotion : MonoBehaviour
    {
        private CharacterController _characterController;
        private PlayerManager _playerManager;

        private Vector3 _currentVelocity; 
        private float _verticalVelocity; 
        private const float Gravity = -9.81f; 
        

        [SerializeField] private float footstepInterval = 0.5f;
        
        private float footstepTimer;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerManager = GetComponent<PlayerManager>();
        }

        public void HandleAllLocomotion()
        {
            HandleMovement();
            HandleFootsteps();
        }

        private void HandleMovement()
        {
            if (!_playerManager.canMove)
            {
                _playerManager.moveVelocity = 0;
                return;
            }
            var targetVelocity = new Vector3(_playerManager.playerInput.x, 0f, _playerManager.playerInput.y).normalized * _playerManager.playerConfig.speed;
            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, Time.deltaTime / _playerManager.playerConfig.accelerationTime);
            if (_characterController.isGrounded)
            {
                _verticalVelocity = 0f; 
            }
            else
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
            var movement = _currentVelocity + Vector3.up * _verticalVelocity;
            _characterController.Move(movement * Time.deltaTime);
            _playerManager.moveVelocity = _characterController.velocity.magnitude;
        }
        
        private void HandleFootsteps()
        {
            if ( _currentVelocity.magnitude > 0.1f)
            {
                footstepTimer -= Time.deltaTime;
                var adjustedInterval = footstepInterval / (_playerManager.playerConfig.speed / 5f);
                if (!(footstepTimer <= 0f)) return;
                PlayFootstepSound();
                footstepTimer = adjustedInterval;
            }
            else
            {
                footstepTimer = Mathf.Max(footstepTimer, footstepInterval);
            }
        }

        
        private void PlayFootstepSound()
        {
            SoundFXChannel.PlaySoundFxClip(_playerManager.footstepClips, transform.position, 1f);
        }
    }
}
