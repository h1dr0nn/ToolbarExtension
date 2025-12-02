using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public static class TimeScaleToolbar
{
    const float TOOLBAR_LABEL_WIDTH = 70f;
    const float TOOLBAR_SLIDER_WIDTH = 100f;
    const float TOOLBAR_FIELD_WIDTH = 35f;
    const float TOOLBAR_HEIGHT = 18f;
    const float TOOLBAR_TOP_OFFSET = 1f;
    const float TOOLBAR_SLIDER_OFFSET_Y = 0f;
    const float TOOLBAR_FIELD_OFFSET_Y = -0.5f;
    const float TOOLBAR_FIELD_PADDING_X = 4f;
    const float TOOLBAR_RIGHT_SPACE = 18f;

    static float timeScale = 1f;

    static TimeScaleToolbar()
    {
        ToolbarExtender.RightToolbarGUI.Insert(0, OnToolbarGUI);
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnToolbarGUI()
    {
        GUILayout.BeginHorizontal(GUILayout.Height(TOOLBAR_HEIGHT));

        var labelStyle = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold
        };

        GUILayout.Label("Timescale", labelStyle, GUILayout.Width(TOOLBAR_LABEL_WIDTH));

        var sliderRect = GUILayoutUtility.GetRect(TOOLBAR_SLIDER_WIDTH, TOOLBAR_HEIGHT);
        sliderRect.y += TOOLBAR_TOP_OFFSET + TOOLBAR_SLIDER_OFFSET_Y;
        timeScale = GUI.HorizontalSlider(sliderRect, timeScale, 0f, 10f);

        timeScale = Mathf.Round(timeScale * 100f) / 100f;

        var fieldRect = GUILayoutUtility.GetRect(TOOLBAR_FIELD_WIDTH, TOOLBAR_HEIGHT);
        fieldRect.y += TOOLBAR_TOP_OFFSET + TOOLBAR_FIELD_OFFSET_Y;
        fieldRect.x += TOOLBAR_FIELD_PADDING_X;
        timeScale = EditorGUI.FloatField(fieldRect, timeScale);

        GUILayout.Space(TOOLBAR_RIGHT_SPACE);
        GUILayout.EndHorizontal();

        if (Application.isPlaying)
            Time.timeScale = timeScale;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
            timeScale = 1f;
    }
}
