using UnityEngine;
using YG;

namespace DefaultNamespace
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private HudScreenController hudScreen;
        [SerializeField] private DocumentSpawner documentSpawner;
        [SerializeField] private GameData gameData;

        private void Awake()
        {
            GameServices.Initialize();

            if (gameData != null) gameData.Initialize();
            if (hudScreen != null) hudScreen.Initialize();
            if (documentSpawner != null) documentSpawner.Initialize();

            YG2.GameReadyAPI();
        }

        private void Start()
        {
            YG2.StickyAdActivity(true);
        }
    }
}