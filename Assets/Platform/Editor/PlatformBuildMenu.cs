using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace GamePlatform.Editor {
public sealed class PlatformBuildMenu : IPreprocessBuildWithReport {
 public int callbackOrder => -2000;
 [MenuItem("Build/Platform/Select Yandex Games")]
 public static void Yandex() => Select(false);
 [MenuItem("Build/Platform/Select CrazyGames")]
 public static void Crazy() => Select(true);
 static void Select(bool crazy) {
 SetResources("Assets/CrazySDK",crazy);
 SetResources("Assets/PluginYourGames",!crazy);
 UnityEditor.Build.Profile.BuildProfile.SetActiveBuildProfile(null);
 var target=UnityEditor.Build.NamedBuildTarget.WebGL;
 var symbols=PlayerSettings.GetScriptingDefineSymbols(target).Split(';').Where(s=>s!="PLATFORM_CRAZYGAMES").ToList();
 if(crazy) symbols.Add("PLATFORM_CRAZYGAMES");
 PlayerSettings.WebGL.template=crazy ? "PROJECT:CrazyGames" : "PROJECT:YandexGames";
 PlayerSettings.SetScriptingDefineSymbols(target,string.Join(";",symbols));
 var editorTarget=UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
 if(editorTarget!=target) PlayerSettings.SetScriptingDefineSymbols(editorTarget,string.Join(";",symbols));
 AssetDatabase.SaveAssets();
 Debug.Log("Platform selected. Wait for compilation, then build WebGL using Build Profiles.");
 }
 static void SetResources(string root,bool enabled) {
 string source=root+(enabled ? "/PlatformResources" : "/Resources");
 string destination=root+(enabled ? "/Resources" : "/PlatformResources");
 if(!AssetDatabase.IsValidFolder(source)) return;
 string error=AssetDatabase.MoveAsset(source,destination);
 if(!string.IsNullOrEmpty(error)) throw new BuildFailedException(error);
 }
 [MenuItem("Build/Platform/Build selected platform")]
 public static void BuildSelected() {
 if(EditorApplication.isCompiling) throw new BuildFailedException("Wait for compilation to finish.");
 var scenes=EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray();
 string folder=PlayerSettings.WebGL.template=="PROJECT:CrazyGames" ? "CrazyGames" : "YandexGames";
 var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes=scenes, target=BuildTarget.WebGL, locationPathName="Builds/"+folder });
 if(result.summary.result!=BuildResult.Succeeded) throw new BuildFailedException("Platform build failed.");
 }
 public void OnPreprocessBuild(BuildReport report) {
 if(report.summary.platform!=BuildTarget.WebGL) return;
#if PLATFORM_CRAZYGAMES
 const string template="PROJECT:CrazyGames";
#else
 const string template="PROJECT:YandexGames";
#endif
 if(PlayerSettings.WebGL.template!=template) throw new BuildFailedException("Platform and WebGL template disagree. Use Build/Platform to select the platform.");
 }
}}
