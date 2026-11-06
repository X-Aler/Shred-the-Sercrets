using System;
using UnityEngine;
namespace GamePlatform {
public static class PlatformServices {
 public static IPlatformService Service { get; private set; }
 public static PlayerProgress Progress { get; private set; }
 public static event Action Ready;
 public static bool IsReady => Progress != null;
 public static void Initialize() {
 if (Service != null) return;
#if PLATFORM_CRAZYGAMES
 Service = new CrazyGamesPlatformService();
#else
 Service = new YandexPlatformService();
#endif
 Service.Initialize(() => { Progress = Service.LoadProgress() ?? new PlayerProgress(); if (Progress.upgradeLevels == null) Progress.upgradeLevels = new int[6]; if (Progress.upgradeLevels.Length < 6) Array.Resize(ref Progress.upgradeLevels,6); Ready?.Invoke(); }, error => Debug.LogError(error));
 }
 public static void Save() { if(IsReady) Service.SaveProgress(Progress); }
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
 static void Reset() { Service=null; Progress=null; Ready=null; }
}}
