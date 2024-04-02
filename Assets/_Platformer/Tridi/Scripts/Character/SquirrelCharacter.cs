using System;
using UnityEngine;

namespace Tridi
{
    public class SquirrelCharacter : Character
    {
        [Serializable]
        public class HealthAttributes
        {
            [SerializeField] public AutoRegenDecorator autoRegen;
            [SerializeField] public DamageCooldownDecorator damageCooldown;
        }
        
        [Header("Squirrel")]
        [SerializeField] private Interactor interactor;
        
        [Space]
        [SerializeField] private HealthAttributes healthAttributes;

        private void Start()
        {
            DecorateHealth(healthAttributes.damageCooldown);
            DecorateHealth(healthAttributes.autoRegen);
        }
        
        protected override void SetupPlayerInput(InputManager inputManager)
        {
            base.SetupPlayerInput(inputManager);
            if (inputManager == null) return;
            
            inputManager.onInteract += OnInteract;
        }

        protected override void RemovePlayerInput(InputManager inputManager)
        {
            base.RemovePlayerInput(inputManager);
            if (inputManager == null) return;
            
            inputManager.onInteract -= OnInteract;
        }
        
        private void OnInteract()
        {
            interactor.Interact();
        }
    }
}

