using System;
using UnityEngine;

namespace Tridi
{
    public class WarriorCharacter : Character
    {
        [Serializable]
        public class HealthAttributes
        {
            [SerializeField] public AutoRegenDecorator autoRegen;
            [SerializeField] public DamageCooldownDecorator damageCooldown;
        }
        
        [Serializable]
        public class ArmorAttributes
        {
            [SerializeField] public AutoRegenDecorator autoRegen;
            [SerializeField] public FlatDamageDecorator flatDamage;
            [SerializeField] public DamageCooldownDecorator damageCooldown;
        }
        
        [Header("Warrior")]
        [SerializeField] private Weapon weapon;
        [Space]
        [SerializeField] private RangedWeapon rangedWeapon;
        [SerializeField] private Transform muzzle;
        [SerializeField, Min(0f)] private float muzzleDistance;
        [Space]
        [SerializeField] private Interactor interactor;
        
        [Header("Decorators")]
        [SerializeField] private HealthAttributes healthAttributes;
        [SerializeField] public ArmorDecorator armor;
        [SerializeField] private ArmorAttributes armorAttributes;
        
        protected virtual void Start()
        {
            DecorateHealth(healthAttributes.damageCooldown);
            DecorateHealth(healthAttributes.autoRegen);
            DecorateHealth(armor);
            
            DecorateArmor(armorAttributes.autoRegen);
            DecorateArmor(armorAttributes.flatDamage);
            DecorateArmor(armorAttributes.damageCooldown);
        }

        public void DecorateArmor(HealthDecorator decorator)
        {
            armor.Decorate(decorator);
        }

        protected override void SetupPlayerInput(InputManager inputManager)
        {
            base.SetupPlayerInput(inputManager);
            if (inputManager == null) return;

            inputManager.onInteract += OnInteract;
            inputManager.onAttack += Attack;
            inputManager.onStartFire += StartFire;
            inputManager.onStopFire += StopFire;
        }

        protected override void RemovePlayerInput(InputManager inputManager)
        {
            base.RemovePlayerInput(inputManager);
            if (inputManager == null) return;
            
            inputManager.onInteract -= OnInteract;
            inputManager.onAttack -= Attack;
            inputManager.onStartFire -= StartFire;
            inputManager.onStopFire -= StopFire;
        }

        private void OnValidate()
        {
            if (!muzzle) return;
            AimAt(transform.position + transform.right * muzzleDistance);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            weapon.onAttack += OnAttack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            weapon.onAttack -= OnAttack;
        }

        private void Update()
        {
            Aim();
        }

        private void OnInteract()
        {
            if (weapon.IsAttacking) return;
            interactor.Interact();
        }
        
        public void Attack()
        {
            if (Movement.IsFalling()) return;
            weapon.Attack();
        }
        
        private void StartFire()
        {
            if (weapon.IsAttacking) return;
            rangedWeapon.StartFire();
        }
        
        private void StopFire()
        {
            rangedWeapon.StopFire();
        }

        private void Aim()
        {
            if (!Camera.main) return;
            if (!InputManager) return;
            
            AimAt(Camera.main.ScreenToWorldPoint(InputManager.Aim));
        }
        
        public void AimAt(Vector2 target)
        {
            var start = (Vector2)transform.position;
            var direction = (target - start).normalized;
            muzzle.position = start + direction * muzzleDistance;
            muzzle.rotation = Quaternion.LookRotation(direction, transform.up);
        }
        
        private void OnAttack(int counter)
        {
            Animator.Attack(counter);
        }

        protected override void OnMove()
        {
            if (weapon.IsAttacking) return;
            base.OnMove();
        }

        protected override void OnJump()
        {
            if (weapon.IsAttacking) return;
            base.OnJump();
        }
    }
}

