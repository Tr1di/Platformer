namespace Tridi
{
    public class InteractiveItem : ItemPlacement, IInteractive
    {
        public int Priority => int.MaxValue;
        
        public void Interact(Interactor instigator)
        {
            if (!instigator) return;
            PickUp(instigator.GetComponent<Inventory>());
        }
    }
}