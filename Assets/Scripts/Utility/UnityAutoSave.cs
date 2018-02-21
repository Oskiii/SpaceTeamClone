/// <summary>
/// AutoSave.cs, editor helper script. Saves the scene ans modified assets when you hit Play in the Unity Editor, so if Unity should 
/// crash, or similar, you don't lose any scene or assets you haddn't saved before hitting Play.
/// </summary>
/// 
/// <remarks>
/// Created (date - name): 10th March 2015 - Gavin Thornton
/// Modified (date - name): 8th Jan 2017 - Gavin Thornton - #if UNITY_EDITOR moved to fix cloud build
/// </remarks>

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
#if false // Change this to "#if true" if you want to disable the script temporarily (without having to delete it)
public class OnUnityLoad
{
static OnUnityLoad()
{
EditorApplication.playmodeStateChanged = () =>
{
if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
Debug.Log("*** WARNING:Auto-Saving DISABLED ***" + EditorApplication.currentScene);
};
}
}

#else
public class OnUnityLoad
{
    static OnUnityLoad()
    {
        EditorApplication.playModeStateChanged += change =>
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying)
            {
                return;
            }
            // Unity 5+
            Debug.Log("Auto-saving scene and any asset edits" + SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();

            // Old code for Unity 4.6 and below users below -------------------
            //Debug.Log("Auto-saving scene " + EditorApplication.currentScene);
            //EditorApplication.SaveScene();
            //EditorApplication.SaveAssets();
        };
    }
}

#endif
#endif // #if UNITY_EDITOR