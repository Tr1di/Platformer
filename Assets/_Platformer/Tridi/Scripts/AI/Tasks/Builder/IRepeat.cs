namespace Tridi.AI
{
    public interface IRepeat
    {
        public ICycling Times { get; }
        public ICycling Time => Times;
        public ICycling Seconds { get; }
        public ICycling Second => Seconds;
    }

    namespace Implementation
    {
        public class RepeatImpl : IRepeat
        {
            private ICycling _cycling;

            public ICycling Times
            {
                get
                {
                    _cycling.Type = RepeatType.Times;
                    return _cycling;
                }
            }

            public ICycling Seconds
            {
                get
                {
                    _cycling.Type = RepeatType.Seconds;
                    return _cycling;
                }
            }

            public RepeatImpl(IAction action, float count)
            {
                _cycling = new CyclingImpl(action, count);
            }
        }
    }
}