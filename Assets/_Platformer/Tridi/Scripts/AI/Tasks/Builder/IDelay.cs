namespace Tridi.AI
{
    public interface IDelay
    {
        public float Duration { get; }

        public IAction Milliseconds { get; }
        public IAction Millisecond => Milliseconds;
        public IAction Seconds { get; }
        public IAction Second => Seconds;
        public IAction Minutes { get; }
        public IAction Minute => Minutes;
    }

    namespace Implementation
    {
        public class DelayImpl : IDelay
        {
            public float Duration { get; private set; }
            public IAction Seconds { get; }
        
            public IAction Milliseconds
            {
                get
                {
                    Duration /= 1000;
                    return Seconds;
                }
            }

            public IAction Minutes
            {
                get
                {
                    Duration *= 60f;
                    return Seconds;
                }
            }

            public DelayImpl(IAction action, float time = 0)
            {
                Seconds = action;
                Duration = time;
            }
        }
    }
}