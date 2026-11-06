#if PLATFORM_CRAZYGAMES
using System;
using CrazyGames;
using UnityEngine;
namespace GamePlatform {
public sealed class CrazyGamesPlatformService : IPlatformService {
 const string Key="shred.progress.v1";
 bool showing;
 float nextInterstitial = 180f;
 public bool IsReady => CrazySDK.IsInitialized;
 public string Language => CrazySDK.User.SystemInfo?.locale ?? "en";
 public void Initialize(Action ready,Action<string> error) { try { CrazySDK.Init(ready); } catch(Exception ex) { error(ex.Message); } }
 public PlayerProgress LoadProgress() { string json=CrazySDK.Data.GetString(Key, ""); return string.IsNullOrEmpty(json) ? new PlayerProgress() : JsonUtility.FromJson<PlayerProgress>(json); }
 public void SaveProgress(PlayerProgress data) => CrazySDK.Data.SetString(Key,JsonUtility.ToJson(data));
 public void GameReady() { }
 public void GameplayStart() => CrazySDK.Game.GameplayStart();
 public void GameplayStop() => CrazySDK.Game.GameplayStop();
 public void ShowInterstitial(Action finished) { if(Time.realtimeSinceStartup < nextInterstitial) { finished?.Invoke(); return; } nextInterstitial=Time.realtimeSinceStartup+180f; Show(CrazyAdType.Midgame,_ => finished?.Invoke()); }
 public void ShowRewarded(string placement,Action<bool> finished) => Show(CrazyAdType.Rewarded,finished);
 void Show(CrazyAdType type,Action<bool> finished) {
 if(showing) { finished?.Invoke(false); return; } showing=true;
 float previousTime=Time.timeScale; bool previousAudio=AudioListener.pause; bool paused=false;
 void Complete(bool success) { if(!showing) return; showing=false; if(paused) { Time.timeScale=previousTime; AudioListener.pause=previousAudio; GameplayStart(); } finished?.Invoke(success); }
 try { CrazySDK.Ad.RequestAd(type,()=> { paused=true; GameplayStop(); Time.timeScale=0; AudioListener.pause=true; },_=>Complete(false),()=>Complete(true)); } catch(Exception ex) { Debug.LogException(ex); Complete(false); }
 }
}}
#endif
