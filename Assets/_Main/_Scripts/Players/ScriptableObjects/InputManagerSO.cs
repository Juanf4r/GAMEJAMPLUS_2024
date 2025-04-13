using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts.Players.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Input/Input Reader", fileName = "InputManager")]
    public class InputManagerSo : ScriptableObject, InputPlayers.IPlayersActions
    {
        
        public event UnityAction<Vector2> PlayerOneMoveEvent = delegate {};
        public event UnityAction<Vector2> PlayerTwoMoveEvent = delegate {};
        public event UnityAction PlayerOnePunchEvent = delegate {};
        public event UnityAction PlayerTwoPunchEvent = delegate {};
        public event UnityAction PlayerOnePowerUpEvent = delegate {};
        public event UnityAction PlayerTwoPowerUpEvent = delegate {};
        public event UnityAction PauseEvent = delegate {};

        private InputPlayers _inputMap;

        private void OnEnable()
        {
            if (_inputMap == null)
            {
                _inputMap = new InputPlayers();
                _inputMap.Players.SetCallbacks(this);
            }
            _inputMap.Enable();
        }
        
        private void OnDisable()
        {
            _inputMap.Disable();
        }
        
        public void OnMovement1(InputAction.CallbackContext context)
        {
            PlayerOneMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMovement2(InputAction.CallbackContext context)
        {
            PlayerTwoMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPunch1(InputAction.CallbackContext context)
        {
            if (context.performed) PlayerOnePunchEvent?.Invoke();
        }

        public void OnPunch2(InputAction.CallbackContext context)
        {
            if (context.performed) PlayerTwoPunchEvent?.Invoke();
        }

        public void OnPowerUp1(InputAction.CallbackContext context)
        {
            if (context.performed) PlayerOnePowerUpEvent?.Invoke();
        }

        public void OnPowerUp2(InputAction.CallbackContext context)
        {
            if (context.performed) PlayerTwoPowerUpEvent?.Invoke();
        }
        
        public void OnPause(InputAction.CallbackContext context)
        {
            PauseEvent?.Invoke();
        }
        
        public void Initialize()
        {
            OnEnable();
        }
    }
}
