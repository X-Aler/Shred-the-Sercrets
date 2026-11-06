#if !PLATFORM_CRAZYGAMES
using System;
using System.Threading.Tasks;
using YG;
namespace GamePlatform {
public sealed class YandexPlatformService : IPlatformService {
 public bool IsReady => YG2.isSDKEnabled;
 public string Language => YG2.envir?.language ?? "en";
 public void Initialize(Action ready, Action<string> error) { if(IsReady) { ready(); return; } void Complete() { YG2.onGetSDKData -= Complete; ready(); } YG2.onGetSDKData += Complete; }
 public PlayerProgress LoadProgress() => new PlayerProgress { coins=YG2.saves.coins, upgradeLevels=YG2.saves.upgradeLevels == null ? new int[6] : (int[])YG2.saves.upgradeLevels.Clone() };
 public void SaveProgress(PlayerProgress data) { YG2.saves.coins=data.coins; YG2.saves.upgradeLevels=(int[])data.upgradeLevels.Clone(); YG2.SaveProgress(); }
 public void GameReady() { YG2.GameReadyAPI(); YG2.StickyAdActivity(true); }
 public void GameplayStart() => YG2.GameplayStart();
 public void GameplayStop() => YG2.GameplayStop();
 public void ShowInterstitial(Action finished) { YG2.InterstitialAdvShow(); finished?.Invoke(); }
 public void ShowRewarded(string placement, Action<bool> finished) {
 if(YG2.nowInterAdv || YG2.nowRewardAdv) { finished(false); return; }
 bool done=false;
 void Cleanup() { done=true; YG2.onRewardAdv -= Reward; YG2.onErrorRewardedAdv -= Error; YG2.onCloseRewardedAdv -= Close; }
 async void Close() { await Task.Yield(); if(!done) Error(); }
 void Reward(string id) { if(done || id!=placement) return; Cleanup(); finished(true); }
 void Error() { if(done) return; Cleanup(); finished(false); }
 YG2.onCloseRewardedAdv += Close; YG2.onRewardAdv += Reward; YG2.onErrorRewardedAdv += Error; YG2.RewardedAdvShow(placement);
 }
}}
#endif
