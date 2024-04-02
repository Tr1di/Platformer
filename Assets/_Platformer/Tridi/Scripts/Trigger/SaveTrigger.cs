using UnityEngine;

namespace Tridi
{
    public class SaveTrigger : Trigger
    {
        public override void Enter(Collider2D other)
        {
            SaveManager.SaveGame();
        }
    }
}

