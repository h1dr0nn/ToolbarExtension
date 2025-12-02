using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class RecompileToolbar
{
    const float TOOLBAR_BTN_WIDTH = 28f;
    const float TOOLBAR_BTN_HEIGHT = 18f;
    const float TOOLBAR_TOP_OFFSET = 1f;
    const float TOOLBAR_RIGHT_SPACE = 4f;

    static Texture2D icon;

    static RecompileToolbar()
    {
        icon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/AfterhoursFoundation/Editor/Toolbars/Icons/reset.png");
        ToolbarExtender.LeftToolbarGUI.Insert(1, OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        var style = new GUIStyle(EditorStyles.toolbarButton)
        {
            fixedHeight = TOOLBAR_BTN_HEIGHT,
            fixedWidth = TOOLBAR_BTN_WIDTH,
            margin = new RectOffset(0, 0, 0, 0)
        };

        var content = icon != null
            ? new GUIContent(icon, "Recompile Scripts")
            : new GUIContent("♻️", "Recompile Scripts");

        var btnRect = GUILayoutUtility.GetRect(TOOLBAR_BTN_WIDTH, TOOLBAR_BTN_HEIGHT, style);
        btnRect.y += TOOLBAR_TOP_OFFSET;

        if (GUI.Button(btnRect, content, style))
        {
            AssetDatabase.Refresh();
            Debug.Log("<color=#4dc3ff>Scripts recompiled!</color>");
        }

        GUILayout.Space(TOOLBAR_RIGHT_SPACE);
    }
}
