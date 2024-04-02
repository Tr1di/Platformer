using System;
using Tridi.AI;
using UnityEditor;
using UnityEngine;

namespace Tridi
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private WarriorCharacter character;
        
        [Header("Combat")]
        [SerializeField] private string enemyTag = "Player";
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float attackRate = 1f;
        [SerializeField] private float attackRange = 2f;

        [Header("Default Tasks")]
        [SerializeField] private FollowTask followTask;
        [SerializeField] private WaitTask waitTask;

        private ITask _attackTask;
        
        [Header("Senses")] 
        [SerializeField] private VisionSense visionSense;

        private Character _enemy; 
        
        private ITask _currentTask;
        
        public void Run(ITask task)
        {
            if (task == _currentTask && task.IsActive) return;
            
            _currentTask?.Cancel();
            _currentTask = task;
            _currentTask?.Execute();
        }

        private void OnEnable()
        {
            character.Health.onDeath += OnDeath;
        }

        private void OnDisable()
        {
            character.Health.onDeath -= OnDeath;
        }

        private void OnDeath(IHealth component, DamageInfo damageinfo)
        {
            Run(null);
        }

        private void FixedUpdate()
        {
            if (!character.IsAlive) return;
            FindEnemy();
            DetermineState();
        }

        private void FindEnemy()
        {
            var enemy = visionSense.See<Character>(enemyTag, enemyLayer);
            if (!enemy)
            {
                _enemy = null;
                return;
            }

            if (!enemy.IsAlive)
            {
                if (_enemy == enemy)
                {
                    _enemy = null;
                    followTask.Cancel();
                    return;
                }
            }
            
            _enemy = enemy;
            followTask.SetTarget(_enemy.transform);
        }
        
        private void Awake()
        {
            _attackTask = Task.Do(character.Attack)
                .While(_ => _enemy && _enemy.IsAlive)
                .Until(_ => _enemy && (_enemy.transform.position - character.transform.position).magnitude >
                    attackRange)
                .For().Every(attackRate).Seconds
                .Prepare();
        }

        private void DetermineState()
        {
            ITask newTask = waitTask; 
            
            if (character.IsAlive)
            {
                if (followTask.IsActive)
                {
                    newTask = followTask;
                }
                
                if (_enemy && _enemy.IsAlive)
                {
                    var distance = (_enemy.transform.position - character.transform.position).magnitude;
                    newTask = distance > attackRange ? followTask : _attackTask;
                }
            }
            else
            {
                newTask = null;
            }

            Run(newTask);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(character.transform.position, visionSense.Distance);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(followTask.Target, followTask.Threshold);
        }

        #region Editor
        #if UNITY_EDITOR

        [MenuItem("Platformer/Add AI Controller")]
        public static void AddAIController()
        {
            var go = new GameObject("AI Controller");
            go.AddComponent<AIController>();
            Selection.activeGameObject = go;
        }
        
        #endif
        #endregion
    }
}
