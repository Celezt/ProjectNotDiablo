//--------------------------------------------------------------------------//
//  RoomPrefab.cs
//  By: Kasper S. Skott
//--------------------------------------------------------------------------//

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Specifies data needed for a room to be used in the DungeonGenerator.
public class RoomPrefab : MonoBehaviour
{
    [System.Serializable]
    public struct MonsterSpawnPoint
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    [Header("Layout")]

    [Range(3, 64)]
    public int width = 3;

    [Range(3, 64)]
    public int height = 3;

    public List<Vector2Int> connections;

    
    [Header("Content")]
    [Space(10)]

    public List<MonsterSpawnPoint> monsterSpawnPoints;

    [Header("Editor")]
    [Space(10)]

    public Mesh monsterSpawnMesh;

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos.y += 0.5f;

        Vector3 rotAxis;
        float rotAngle;
        transform.rotation.ToAngleAxis(out rotAngle, out rotAxis);
        float w;
        float h;
        if (Mathf.Abs(rotAngle) % 180.0f > 89.99f) {
            w = height;
            h = width;
        }
        else {
            w = width;
            h = height;
        }

        // Visualize room bounds
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(pos, new Vector3(w, 1.0f, h));

        // Visualize connections        
        Gizmos.color = Color.green;
        for (int i = 0; i < connections.Count; i++) {
            pos = new Vector3(connections[i].x, 0.5f, connections[i].y);
            pos = transform.position + transform.rotation * pos;

            Gizmos.DrawWireCube(pos, new Vector3(1.0f, 1.0f, 1.0f));
        }

        // Visualize monster spawn points
        Gizmos.color = Color.red;
        for (int i = 0; i < monsterSpawnPoints.Count; i++) {
            Quaternion rot = Quaternion.Euler(monsterSpawnPoints[i].rotation);
            pos = new Vector3(monsterSpawnPoints[i].position.x, 
                monsterSpawnPoints[i].position.y, 
                monsterSpawnPoints[i].position.z);
            pos = transform.position + transform.rotation * pos;

            Gizmos.DrawMesh(monsterSpawnMesh, -1, pos, rot, new Vector3(1.0f, 1.0f, 1.0f));
        }
    }

    // Returns {width, height} rotated by 'angle' degrees.
    // Only 90-degree increments are supported.
    public Vector2Int GetRotatedBounds(float angle) {
        Vector2Int ret = new Vector2Int();
        float absAngle = Mathf.Abs(angle) % 180.0f;
        if (absAngle > 89.9f) {
            ret.x = height;
            ret.y = width;
        }
        else {
            ret.x = width;
            ret.y = height;
        }

        return ret;
    }

    // Returns a new list of connections that are rotated by 'angle' degrees.
    // Angle may be negative, but should not exceed 180, negative or positive.
    public List<Vector2Int> GetRotatedConnections(float angle) {
        List<Vector2Int> ret = new List<Vector2Int>(connections.Count);
        Quaternion rot = Quaternion.AngleAxis(angle,
            new Vector3(0.0f, 1.0f, 0.0f));

        Vector3 pos;
        for (int i = 0; i < connections.Count; i++) {
            pos = new Vector3(connections[i].x, 0.0f, connections[i].y);
            pos = rot * pos;

            ret.Add(new Vector2Int((int)pos.x, (int)pos.z));
        }

        return ret;
    }

}
