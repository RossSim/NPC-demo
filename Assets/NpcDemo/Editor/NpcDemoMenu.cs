using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RossSim.NpcDemo.Editor
{
    public static class NpcDemoMenu
    {
        const string ScenePath = "Assets/NpcDemo/Scenes/NpcYard.unity";
        public const string MacPlayerPath = "Builds/macOS/NPC-demo.app";

        [MenuItem("NPC Demo/Create Yard Scene")]
        public static void CreateYardScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var root = new GameObject("NpcDemo");
            root.AddComponent<NpcDemoHud>();
            System.IO.Directory.CreateDirectory("Assets/NpcDemo/Scenes");
            EditorSceneManager.SaveScene(scene, ScenePath);
            var build = new EditorBuildSettingsScene(ScenePath, true);
            EditorBuildSettings.scenes = new[] { build };
            Debug.Log("Saved " + ScenePath + " and set it as the only build scene.");
        }

        [MenuItem("NPC Demo/Use .NET Standard 2.1")]
        public static void SetApiCompat()
        {
            PlayerSettings.SetApiCompatibilityLevel(
                NamedBuildTarget.Standalone,
                ApiCompatibilityLevel.NET_Standard);
            Debug.Log("Standalone API Compatibility set to .NET Standard 2.1.");
        }

        [MenuItem("NPC Demo/Build Mac Player")]
        public static void BuildMacPlayer()
        {
            BuildMac(BuildOptions.None);
        }

        [MenuItem("NPC Demo/Build and Run Mac Player")]
        public static void BuildAndRunMacPlayer()
        {
            BuildMac(BuildOptions.AutoRunPlayer);
        }

        static void BuildMac(BuildOptions options)
        {
            SetApiCompat();
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            Directory.CreateDirectory(Path.GetDirectoryName(MacPlayerPath));
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = MacPlayerPath,
                target = BuildTarget.StandaloneOSX,
                options = options
            });
            if (report.summary.result != BuildResult.Succeeded)
                throw new System.Exception("Mac player build failed: " + report.summary.result);
            Debug.Log("Built " + Path.GetFullPath(MacPlayerPath));
        }
    }
}
