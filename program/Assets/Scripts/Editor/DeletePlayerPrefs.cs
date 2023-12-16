using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class DeletePlayerPrefs : EditorWindow
{
    [MenuItem("Window/Delete PlayerPrefs")]
    private static void OpenWindow()
    {
        DeletePlayerPrefs window = GetWindow<DeletePlayerPrefs>();
        window.titleContent = new GUIContent("Delete PlayerPrefs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Button Window", EditorStyles.boldLabel);

        if (GUILayout.Button("Click Me"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
