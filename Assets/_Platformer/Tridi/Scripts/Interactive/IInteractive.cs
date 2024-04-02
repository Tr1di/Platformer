namespace Tridi
{
    public interface IInteractive
    {
        int Priority { get; }
    
        void Interact(Interactor instigator);
    }
}

