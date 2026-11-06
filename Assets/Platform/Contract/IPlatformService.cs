using System;

namespace GamePlatform
{
    /// <summary>Services supplied by the selected distribution platform.</summary>
    public interface IPlatformService
    {
        bool IsReady { get; }
        string Language { get; }
        void Initialize(Action onReady, Action<string> onError);
        PlayerProgress LoadProgress();
        void SaveProgress(PlayerProgress progress);
        void GameReady();
        void GameplayStart();
        void GameplayStop();
        void ShowInterstitial(Action onFinished);
        void ShowRewarded(string placement, Action<bool> onFinished);
    }
}
