using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;
using System.IO;

[InitializeOnLoad]
public static class PlayFromLoadingToolbar
{
    const float TOOLBAR_BTN_WIDTH = 120f;
    const float TOOLBAR_BTN_HEIGHT = 18f;
    const float TOOLBAR_TOP_OFFSET = 1f;
    const float TOOLBAR_RIGHT_SPACE = 18f;
    const string LOADING_SCENE_PATH = "Assets/_h1dr0n/Scenes/LoadingScene.unity";
    const string LAST_SCENE_KEY = "h1dr0n_last_scene_path";
    const string PRESSED_KEY = "h1dr0n_pressed_toolbar_flag";

    static PlayFromLoadingToolbar()
    {
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        var style = new GUIStyle(EditorStyles.toolbarButton)
        {
            fixedHeight = TOOLBAR_BTN_HEIGHT,
            fixedWidth = TOOLBAR_BTN_WIDTH,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        var rect = GUILayoutUtility.GetRect(TOOLBAR_BTN_WIDTH, TOOLBAR_BTN_HEIGHT, style);
        rect.y += TOOLBAR_TOP_OFFSET;

        if (GUI.Button(rect, new GUIContent("PLAY", "Play from Loading Scene"), style))
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            if (!File.Exists(LOADING_SCENE_PATH))
            {
                Debug.LogError($"[PlayFromLoadingToolbar] Scene not found at: {LOADING_SCENE_PATH}");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            string currentScene = EditorSceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(currentScene))
                SessionState.SetString(LAST_SCENE_KEY, currentScene);

            SessionState.SetBool(PRESSED_KEY, true);

            EditorSceneManager.OpenScene(LOADING_SCENE_PATH);
            EditorApplication.isPlaying = true;
        }

        GUILayout.Space(TOOLBAR_RIGHT_SPACE);
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                {
                    bool wasPressed = SessionState.GetBool(PRESSED_KEY, false);
                    if (wasPressed)
                    {
                        SessionState.SetBool(PRESSED_KEY, false);

                        string lastScenePath = SessionState.GetString(LAST_SCENE_KEY, string.Empty);
                        if (!string.IsNullOrEmpty(lastScenePath) && File.Exists(lastScenePath))
                        {
                            EditorApplication.delayCall += () =>
                            {
                                EditorSceneManager.OpenScene(lastScenePath);
                            };
                        }
                    }
                    break;
                }
        }
    }
}
