//Source: https://gist.github.com/edatencio/7ddb1410e92d11617ad333c9efa7ef83/
//Original: https://forum.unity3d.com/threads/we-need-auto-save-feature.483853/

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class Autosave
{
    static Autosave()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                Debug.Log("Auto-saving all open scenes...");
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        };
    }
}
