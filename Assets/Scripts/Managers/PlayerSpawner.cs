using UnityEditor;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public float radius = 0.5f;
    public float height = 2f;
    public Color gizmoColor = Color.green;

    private void OnEnable()
    {
        GameEvents.StartGame += SpawnPlayer;
    }
    private void OnDisable()
    {
        GameEvents.StartGame -= SpawnPlayer;
    }
    public void SpawnPlayer()
    {
        GameManager.instance.SpawnPlayer(transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 top = transform.position + Vector3.up * (height / 2 - radius);
        Vector3 bottom = transform.position - Vector3.up * (height / 2 - radius);
        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);

        DrawCylinder(bottom, top, radius);
    }

    private void DrawCylinder(Vector3 start, Vector3 end, float radius)
    {
        Vector3 direction = (end - start).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 side = Vector3.Cross(direction, Vector3.right).normalized * radius;


        for (int i = 0; i < 12; i++)
        {
            float angle = (i / 12f) * Mathf.PI * 2;
            float nextAngle = ((i + 1) / 12f) * Mathf.PI * 2;

            Vector3 offset1 = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Vector3 offset2 = new Vector3(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle), 0) * radius;

    
            Vector3 point1Start = start + rotation * offset1;
            Vector3 point2Start = start + rotation * offset2;
            Vector3 point1End = end + rotation * offset1;
            Vector3 point2End = end + rotation * offset2;

            Gizmos.DrawLine(point1Start, point2Start);
            Gizmos.DrawLine(point1End, point2End);
            Gizmos.DrawLine(point1Start, point1End);
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerSpawner))]
public class SpawnPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PlayerSpawner spawner = (PlayerSpawner)target;
        if (GUILayout.Button("Spawn Player"))
        {
            spawner.SpawnPlayer();
        }
    }
}
#endif