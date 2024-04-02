using System;
using System.Collections.Generic;
using System.Linq;
using Tridi;

namespace Tridi
{
    [Serializable]
    public class SaveGame
    {
        public long lastUpdated = DateTime.Now.ToBinary();
        public List<Character.SaveData> characters = new();
        public List<Health.SaveData> healthes = new();
    
        public void SaveCharacter(Character.SaveData data)
        {
            characters.RemoveAll(x => x.id == data.id);
            characters.Add(data);
        }

        public Character.SaveData GetCharacter(string id)
        {
            return characters.LastOrDefault(x => x.id == id);
        }
        
        public void SaveHealth(Health.SaveData data)
        {
            healthes.RemoveAll(x => x.id == data.id);
            healthes.Add(data);
        }

        public Health.SaveData GetHealth(string id)
        {
            return healthes.LastOrDefault(x => x.id == id);
        }
    }
}


