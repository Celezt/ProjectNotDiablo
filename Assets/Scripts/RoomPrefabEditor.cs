using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(RoomPrefab))]
public class RoomPrefabEditor : Editor
{
    SerializedProperty tileSize;
    SerializedProperty width;
    SerializedProperty height;
    SerializedProperty connections;
    SerializedProperty monsterSpawnPoints;
    SerializedProperty monsterPool;
    SerializedProperty minMonsters;
    SerializedProperty maxMonsters;
    SerializedProperty playerSpawnPoint;

    private bool editMode;

    void OnEnable()
    {
        tileSize = serializedObject.FindProperty("tileSize");
        width = serializedObject.FindProperty("width");
        height = serializedObject.FindProperty("height");
        connections = serializedObject.FindProperty("connections");
        monsterSpawnPoints = serializedObject.FindProperty("monsterSpawnPoints");
        monsterPool = serializedObject.FindProperty("monsterPool");
        minMonsters = serializedObject.FindProperty("minMonsters");
        maxMonsters = serializedObject.FindProperty("maxMonsters");
        playerSpawnPoint = serializedObject.FindProperty("playerSpawnPoint");
    }

    public override void OnInspectorGUI()
    {
        RoomPrefab roomPrefab = target as RoomPrefab;

        serializedObject.Update();

        bool editModeVal = false;
        editModeVal = EditorGUILayout.Toggle("Edit Mode", editMode);

        if (editMode != editModeVal) {
            if (roomPrefab.transform.rotation == Quaternion.identity)
                editMode = editModeVal;
            else
                Debug.LogWarning("You can't enable edit mode while rotated.");
        }

        EditorGUILayout.Space(20.0f);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Tile Size");
            tileSize.floatValue = EditorGUILayout.FloatField(tileSize.floatValue, GUILayout.Width(30.0f));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("Room Dimensions");
            width.intValue = EditorGUILayout.IntField(width.intValue, GUILayout.Width(30.0f));
            EditorGUILayout.LabelField("x", GUILayout.Width(10.0f));
            height.intValue = EditorGUILayout.IntField(height.intValue, GUILayout.Width(30.0f));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10.0f);

        EditorGUILayout.PropertyField(connections, new GUIContent("Corridor Connection Points"));

        EditorGUILayout.Space(10.0f);

        EditorGUILayout.PropertyField(monsterPool, new GUIContent("Monster Pool"));

        EditorGUILayout.PropertyField(minMonsters, new GUIContent("Minimum Monsters"));
        EditorGUILayout.PropertyField(maxMonsters, new GUIContent("Maximum Monsters"));

        EditorGUILayout.PropertyField(monsterSpawnPoints, new GUIContent("Monster Spawn Points"));

        EditorGUILayout.Space(10.0f);

        EditorGUILayout.PropertyField(playerSpawnPoint, new GUIContent("Player Spawn Point"));

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        if (!editMode)
            return;

        RoomPrefab roomPrefab = target as RoomPrefab;

        // Connection handles

        EditorGUI.BeginChangeCheck();

        List<Vector2Int> newConnections = new List<Vector2Int>(roomPrefab.connections.Count);

        for (int i = 0; i < roomPrefab.connections.Count; i++) {
            Vector3 pos = new Vector3(roomPrefab.connections[i].x * roomPrefab.tileSize, 0.0f,
                roomPrefab.connections[i].y * roomPrefab.tileSize);
            pos += roomPrefab.transform.position;

            pos = Handles.PositionHandle(pos, Quaternion.identity);
            pos -= roomPrefab.transform.position;
            pos /= roomPrefab.tileSize;

            newConnections.Add(new Vector2Int((int)pos.x, (int)pos.z));
        }

        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(roomPrefab, "Move connection");

            if (roomPrefab.transform.rotation == Quaternion.identity)
                roomPrefab.connections = newConnections;
            else
                Debug.LogWarning("Connections can't be edited while rotated.");
        }

        // Monster spawn point handles

        RoomPrefab.MonsterSpawnPoint
        GetTransformedSpawnPoint(RoomPrefab.MonsterSpawnPoint spawnPoint)
        {
            Vector3 pos = new Vector3(
                spawnPoint.position.x * roomPrefab.tileSize,
                spawnPoint.position.y * roomPrefab.tileSize,
                spawnPoint.position.z * roomPrefab.tileSize);
            pos += roomPrefab.transform.position;

            Quaternion rot = Quaternion.Euler(spawnPoint.rotation);
            Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

            Handles.TransformHandle(ref pos, ref rot, ref scale);
            pos -= roomPrefab.transform.position;
            pos /= roomPrefab.tileSize;

            var point = new RoomPrefab.MonsterSpawnPoint();
            point.position = pos;
            point.rotation = rot.eulerAngles;

            return point;
        }

        EditorGUI.BeginChangeCheck();

        var spawnPoints = new List<RoomPrefab.MonsterSpawnPoint>(roomPrefab.monsterSpawnPoints.Count);
        var playerSpawnPoint = new RoomPrefab.MonsterSpawnPoint();

        for (int i = 0; i < roomPrefab.monsterSpawnPoints.Count; i++) {
            var point = GetTransformedSpawnPoint(roomPrefab.monsterSpawnPoints[i]);
            spawnPoints.Add(point);
        }

        playerSpawnPoint = GetTransformedSpawnPoint(roomPrefab.playerSpawnPoint);

        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(roomPrefab, "Move monster spawn point");

            if (roomPrefab.transform.rotation == Quaternion.identity) {
                roomPrefab.monsterSpawnPoints = spawnPoints;
                roomPrefab.playerSpawnPoint = playerSpawnPoint;
            }
            else
                Debug.LogWarning("Spawn points can't be edited while rotated.");
        }

    }
}
#endif
