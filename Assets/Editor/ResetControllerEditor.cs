using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResetAreaController))]
public class ResetControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResetAreaController myScript = (ResetAreaController)target;
        if (GUILayout.Button("Sync Objects"))
        {
            myScript.SyncObjects();
        }
    }
}