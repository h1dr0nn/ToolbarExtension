using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class ClearPlayerPrefsToolbar
{
    const float TOOLBAR_BTN_WIDTH = 28f;
    const float TOOLBAR_BTN_HEIGHT = 18f;
    const float TOOLBAR_TOP_OFFSET = 1f;
    const float TOOLBAR_RIGHT_SPACE = 0f;

    static Texture2D icon;

    static ClearPlayerPrefsToolbar()
    {
        icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/AfterhoursFoundation/Editor/Toolbars/Icons/trash-can.png");
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        var style = new GUIStyle(EditorStyles.toolbarButton)
        {
            fixedHeight = TOOLBAR_BTN_HEIGHT,
            fixedWidth = TOOLBAR_BTN_WIDTH,
            margin = new RectOffset(0, 0, 0, 0)
        };

        var defaultColor = GUI.color;
        if (Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            GUI.color = Color.white * 1.2f;

        var content = icon != null
            ? new GUIContent(icon, "Clear PlayerPrefs")
            : new GUIContent("🗑️", "Clear PlayerPrefs");

        var btnRect = GUILayoutUtility.GetRect(TOOLBAR_BTN_WIDTH, TOOLBAR_BTN_HEIGHT, style);
        btnRect.y += TOOLBAR_TOP_OFFSET;

        if (GUI.Button(btnRect, content, style))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("<color=#f25b5b>PlayerPrefs cleared!</color>");
        }

        GUI.color = defaultColor;
        GUILayout.Space(TOOLBAR_RIGHT_SPACE);
    }
}

