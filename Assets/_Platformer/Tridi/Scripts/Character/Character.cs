using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tridi
{
    [RequireComponent(typeof(Health))]
    public class Character : MonoBehaviour, IDamageable, ISavable
    {
        [Serializable]
        public class SaveData
        {
            [SerializeField] public string id;
            [SerializeField] public Vector3 position;
        }
        
        [Header("Character")]
        [Id]
        [SerializeField] private string id;
        [Space]
        [SerializeField] private Movement movement;
        [Space]
        [SerializeField] private CharacterAnimator characterAnimator;
        
        private IHealth _health;
        private InputManager _inputManager;

        public bool IsAlive => _health.IsAlive;

        public IHealth Health => _health;
        
        public Movement Movement => movement;
        public CharacterAnimator Animator => characterAnimator;
        
        #region Input Setup
        
        public InputManager InputManager
        {
            get => _inputManager;
            set
            {
                RemovePlayerInput(_inputManager);
                _inputManager = value;
                SetupPlayerInput(_inputManager);
            }
        }

        protected virtual void SetupPlayerInput(InputManager inputManager)
        {
            if (inputManager == null) return;
            
            RemovePlayerInput(inputManager);
            
            inputManager.onJump += OnJump;
        }

        protected virtual void RemovePlayerInput(InputManager inputManager)
        {
            if (inputManager == null) return;

            inputManager.onJump -= OnJump;
        }

        #endregion

        #region Unity Events

        protected virtual void OnEnable()
        {
            SetupPlayerInput(_inputManager);

            _health.onDeath += OnDeath;
            _health.onDamage += OnDamage;
        }
        
        protected virtual void OnDisable()
        {
            RemovePlayerInput(_inputManager);
            
            _health.onDeath -= OnDeath;
            _health.onDamage -= OnDamage;
        }

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
        }

        protected virtual void FixedUpdate()
        {
            OnMove();
        }

        #endregion
        
        protected virtual void OnMove()
        {
            if (!_inputManager) return;
            if (!_health.IsAlive) return;
            
            movement.Move(_inputManager.Move);
        }
        
        protected virtual void OnJump()
        { 
            movement.DoJump();
        }
        
        #region Damageable
        
        public virtual float TakeDamage(DamageInfo damageInfo)
        {
            return _health.TakeDamage(damageInfo);
        }

        private void OnDeath(IHealth component, DamageInfo damageinfo)
        {
            RemovePlayerInput(_inputManager);
            characterAnimator.Death();
        }
        
        private void OnDamage(IHealth component, DamageInfo damageinfo)
        {
            characterAnimator.Hurt();
        }
        
        #endregion

        public void DecorateHealth(HealthDecorator healthDecorator)
        {
            _health = healthDecorator.Assign(_health);
        }

        #region SaveLoad
        
        public void LoadGame(SaveGame game)
        {
            SetupPlayerInput(_inputManager);
            var data = game.GetCharacter(id);
            
            if (data == null) return;
            
            transform.position = data.position;
            Load(data);
        }

        protected virtual void Load(SaveData data)
        { }
        
        public void SaveGame(SaveGame game)
        {
            var data = Save();
            data.id = id;
            data.position = transform.position;
            
            game.SaveCharacter(data);
        }

        protected virtual SaveData Save()
        {
            return new SaveData();
        }
        
        #endregion
        
        
    }
}