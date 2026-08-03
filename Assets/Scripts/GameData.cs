using UnityEngine;

namespace DefaultNamespace
{
    public class GameData : MonoBehaviour
    {
        public static GameData Instance { get; private set; }
        
        public int DocumentsSpawnTime = 30;
        public void Initialize()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    }
}