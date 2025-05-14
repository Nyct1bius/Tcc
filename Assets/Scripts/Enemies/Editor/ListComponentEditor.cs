using UnityEngine;
using UnityEditor;
using UnityEngine.ProBuilder.Shapes;

[CustomEditor(typeof(RoomManager))]
public class ListComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the script
        RoomManager myTarget = (RoomManager)target;
        // Draw the default inspector first (includes all public fields)
        myTarget.isWavedRoom = EditorGUILayout.Toggle("It's a Waved Room", myTarget.isWavedRoom);
        

        // If toggle is true, show the list
        if (myTarget.isWavedRoom)
        {
            SerializedProperty listProperty = serializedObject.FindProperty("enemiesWave1");
            EditorGUILayout.PropertyField(listProperty, true); // 'true' to allow children (i.e., elements of list)
            listProperty = serializedObject.FindProperty("enemiesWave2");
            EditorGUILayout.PropertyField(listProperty, true); // 'true' to allow children (i.e., elements of list)
            myTarget.isThereWave3 = EditorGUILayout.Toggle("There is a Wave 3", myTarget.isThereWave3);

            if (myTarget.isThereWave3)
            {
                listProperty = serializedObject.FindProperty("enemiesWave3");
                EditorGUILayout.PropertyField(listProperty, true); // 'true' to allow children (i.e., elements of list)
            }

            SerializedProperty closingWallsProperty = serializedObject.FindProperty("closingWalls");
            EditorGUILayout.PropertyField(closingWallsProperty, true); // 'true' to allow children (i.e., elements of list)
        }
        else
        {
            SerializedProperty listProperty = serializedObject.FindProperty("MyEnemies");
            EditorGUILayout.PropertyField(listProperty, true); // 'true' to allow children (i.e., elements of list)
        }

        SerializedProperty doorProperty = serializedObject.FindProperty("door");
        EditorGUILayout.PropertyField(doorProperty, true); // 'true' to allow children (i.e., elements of list)

        

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }

}
