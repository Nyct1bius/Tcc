using UnityEditor;
using UnityEngine;

public class EditorScreenShots : MonoBehaviour
{

}

[CustomEditor(typeof(EditorScreenShots))]
public class EditorScreenShotsEditor : Editor
{
    public string textureName = "Minimap_";
    public string path = "Assets/UI";
    static int counter;

    public override void OnInspectorGUI()
    {
        textureName = EditorGUILayout.TextField("Name:", textureName);
        path = EditorGUILayout.TextField("Path:", path);
        if (GUILayout.Button("Capture"))
        {
            Screenshot();
        }
    }

    //[MenuItem("Screenshot/Take screenshot")]
    void Screenshot()
    {
        ScreenCapture.CaptureScreenshot(path + textureName + counter + ".png");
        counter++;
    }
}