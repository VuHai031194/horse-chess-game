using UnityEngine;
using UnityEditor;
#if UNITY_5_3
using UnityEditor.SceneManagement;
#endif
using System.IO;

[InitializeOnLoad]
public class AutoRun{

    static AutoRun()
    {
        EditorApplication.update += InitProject;
        //Debug.LogError("Check");
    }

    static void InitProject()
    {
        //EditorPrefs.SetBool("AlreadyOpened", false);
        EditorApplication.update -= InitProject;
        if (EditorApplication.timeSinceStartup < 10 || !EditorPrefs.GetBool("AlreadyOpened"))
        {
#if UNITY_5_3
            if (EditorSceneManager.GetActiveScene().name != "Game" && Directory.Exists("Assets/GamePattern/Scenes"))
            {
                EditorSceneManager.OpenScene("Assets/GamePattern/Scenes/Game.unity");

            }
#else
            if (Application.loadedLevelName == "Game" && Directory.Exists("Assets/GamePattern/Scenes"))
            {
                EditorApplication.OpenScene("Assets/GamePattern/Scenes/Game.unity");
            }
#endif
            PatternMakerEditor.Init();
            //PatternMakerEditor.ShowHelp();
            EditorPrefs.SetBool("AlreadyOpened", true);
        }
    }
}
