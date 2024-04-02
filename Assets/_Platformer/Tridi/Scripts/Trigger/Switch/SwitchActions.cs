using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tridi
{
    [Serializable]
    public class SwitchActions
    {
        [Serializable]
        public enum SwitchType
        {
            Once,
            Loop,
            PingPong
        }

        [SerializeField] private SwitchType type = SwitchType.Loop;
        [SerializeField] private int state;
        [Space]
        [SerializeField] private UnityEvent[] actions;
        
        public int State => type switch
        {
            SwitchType.Once => state,
            SwitchType.Loop => state % actions.Length,
            SwitchType.PingPong => (int)Mathf.PingPong(state, actions.Length - 1),
            _ => throw new ArgumentOutOfRangeException()
        };

        public bool Done => type == SwitchType.Once && State > actions.Length - 1;
        
        public void Invoke()
        {
            if (Done) return;
            actions[State]?.Invoke();
            state++; 
        }
    }
}