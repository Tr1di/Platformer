using UnityEngine;

namespace Tridi.AI
{
    public enum RepeatType
    {
        Times,
        Seconds
    }
    
    public interface ICycling
    {
        public RepeatType Type { get; set; }
        public float Duration { get; }
        public float Update { get; }

        public IDelay Every(float seconds);
        
        public IAction EveryFixedUpdate()
        {
            return Every(Time.fixedDeltaTime).Seconds;
        }
    }

    namespace Implementation
    {
        public class CyclingImpl : ICycling
        {
            private readonly IAction _action;
            
            private IDelay _delay;
            public RepeatType Type { get; set; }
            public float Duration { get; }
            public float Update => _delay.Duration;
        
            public CyclingImpl(IAction action, float count = 0)
            {
                _action = action;
                Duration = count;
                _delay = new DelayImpl(action, Time.fixedDeltaTime);
            }

            public IDelay Every(float seconds)
            {
                _delay = new DelayImpl(_action, seconds);
                return _delay;
            }
        }
    }
}