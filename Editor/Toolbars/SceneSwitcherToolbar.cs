using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace h1dr0n
{
    [InitializeOnLoad]
    public static class SceneDropdownToolbar
    {
        const float TOOLBAR_BTN_WIDTH = 200f;
        const float TOOLBAR_BTN_HEIGHT = 18f;
        const float TOOLBAR_TOP_OFFSET = 1f;
        const float TOOLBAR_LEFT_SPACE = 18f;
        const float TOOLBAR_RIGHT_SPACE = 0f;

        static readonly List<(string path, string displayName)> scenes = new();
        static int selectedIndex;

        static SceneDropdownToolbar()
        {
            RefreshScenes();
            SyncActiveScene();
            ToolbarExtender.LeftToolbarGUI.Insert(0, OnToolbarGUI);
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorApplication.projectChanged += RefreshScenes;
        }

        static void RefreshScenes()
        {
            scenes.Clear();
            var allScenes = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
            foreach (var path in allScenes)
            {
                var cleanPath = path.Replace("\\", "/");
                var folder = Path.GetDirectoryName(cleanPath)?.Replace("\\", "/")?.Replace("Assets/", "");
                var name = Path.GetFileNameWithoutExtension(cleanPath);
                var display = string.IsNullOrEmpty(folder) ? name : $"{folder}/{name}";
                scenes.Add((cleanPath, display));
            }

            SyncActiveScene();
        }

        static void SyncActiveScene()
        {
            var activePath = EditorSceneManager.GetActiveScene().path;
            if (string.IsNullOrEmpty(activePath))
            {
                selectedIndex = 0;
                return;
            }

            var index = scenes.FindIndex(s => s.path == activePath);
            selectedIndex = index >= 0 ? index : 0;
        }

        static void OnSceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            SyncActiveScene();
        }

        static void OnToolbarGUI()
        {
            GUILayout.Space(TOOLBAR_LEFT_SPACE);

            if (scenes.Count == 0)
            {
                if (GUILayout.Button("No Scenes Found", EditorStyles.toolbarButton, GUILayout.Width(TOOLBAR_BTN_WIDTH)))
                    RefreshScenes();
                return;
            }

            var names = scenes.Select(s => s.displayName).ToArray();
            var style = new GUIStyle(EditorStyles.toolbarPopup)
            {
                fixedHeight = TOOLBAR_BTN_HEIGHT,
                fixedWidth = TOOLBAR_BTN_WIDTH,
                alignment = TextAnchor.MiddleLeft
            };

            var rect = GUILayoutUtility.GetRect(TOOLBAR_BTN_WIDTH, TOOLBAR_BTN_HEIGHT, style);
            rect.y += TOOLBAR_TOP_OFFSET;

            EditorGUI.BeginChangeCheck();
            var newIndex = EditorGUI.Popup(rect, selectedIndex, names, style);
            if (EditorGUI.EndChangeCheck() && newIndex >= 0 && newIndex < scenes.Count)
            {
                selectedIndex = newIndex;
                var path = scenes[selectedIndex].path;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(path);
            }

            GUILayout.Space(TOOLBAR_RIGHT_SPACE);
        }
    }
}
