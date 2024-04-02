using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tridi
{
    public interface ISavable
    {
        void LoadGame(SaveGame data);

        // The 'ref' keyword was removed from here as it is not needed.
        // In C#, non-primitive types are automatically passed by reference.
        void SaveGame(SaveGame data);
    }
}