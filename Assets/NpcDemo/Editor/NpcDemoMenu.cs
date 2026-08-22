using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RossSim.NpcDemo.Editor
{
    public static class NpcDemoMenu
    {
        const string ScenePath = "Assets/NpcDemo/Scenes/NpcYard.unity";

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
    }
}
