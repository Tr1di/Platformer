using UnityEngine;

namespace Tridi
{
    public class InputManager : MonoBehaviour
    {
        private Controls _controls;

        public Vector2 Move => _controls.Player.Move.ReadValue<Vector2>();
        public Vector2 Aim => _controls.Player.Aim.ReadValue<Vector2>();
        
        public delegate void OnAction();
        public event OnAction onJump;
        public event OnAction onInteract;
        public event OnAction onAttack;
        public event OnAction onStartFire;
        public event OnAction onStopFire;
        
        public event OnAction onPause;
        
        private void Awake()
        {
            _controls = new Controls();
            _controls.Player.Jump.performed += _ => onJump?.Invoke();
            _controls.Player.Interact.performed += _ => onInteract?.Invoke();
            _controls.Player.Attack.performed += _ => onAttack?.Invoke();
            
            _controls.Player.Fire.started += _ => onStartFire?.Invoke();
            _controls.Player.Fire.canceled += _ => onStopFire?.Invoke();

            _controls.UI.Pause.performed += _ => onPause?.Invoke();
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void OnDestroy()
        {
            _controls.Dispose();
        }
    }
}