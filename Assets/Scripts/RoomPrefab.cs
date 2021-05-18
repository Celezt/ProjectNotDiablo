//--------------------------------------------------------------------------//
//  RoomPrefab.cs
//  By: Kasper S. Skott
//--------------------------------------------------------------------------//

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Specifies data needed for a room to be used in the DungeonGenerator.
[ExecuteInEditMode]
public class RoomPrefab : MonoBehaviour
{
    [System.Serializable]
    public struct MonsterSpawnPoint
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    [Min(0.0f)]
    public float tileSize = 1.0f;

    [Range(3, 64)]
    public int width = 3;

    [Range(3, 64)]
    public int height = 3;

    public List<Vector2Int> connections;

    public List<MonsterSpawnPoint> monsterSpawnPoints;

    public MonsterPool monsterPool;

    public int minMonsters;

    public int maxMonsters;
    
    public MonsterSpawnPoint playerSpawnPoint;

    //----------------------------------------------------------------------//

    private Mesh monsterSpawnMesh;
    private GameObject monstersParent;

    //----------------------------------------------------------------------//

    void Awake()
    {
    #if UNITY_EDITOR
        monsterSpawnMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Meshes/Editor/SpawnPoint.obj");
    #endif

        if (monstersParent == null) {
            monstersParent = GameObject.Find("Monsters");
            if (monstersParent == null)
                monstersParent = new GameObject("Monsters");
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 tileDimensions = new Vector3(tileSize, tileSize, tileSize);

        Vector3 pos = transform.position;
        pos.y += tileSize / 2.0f;

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
        Gizmos.DrawWireCube(pos, new Vector3(w * tileSize, tileSize, h * tileSize));

        // Visualize connections        
        Gizmos.color = Color.green;
        for (int i = 0; i < connections.Count; i++) {
            pos = new Vector3(connections[i].x, 0.5f, connections[i].y);
            pos = transform.position + transform.rotation * pos * tileSize;

            Gizmos.DrawWireCube(pos, tileDimensions);
        }

        // Visualize monster spawn points
        Gizmos.color = Color.red;
        for (int i = 0; i < monsterSpawnPoints.Count; i++) {
            Quaternion rot = Quaternion.Euler(monsterSpawnPoints[i].rotation) * transform.rotation;
            pos = new Vector3(monsterSpawnPoints[i].position.x, 
                monsterSpawnPoints[i].position.y, 
                monsterSpawnPoints[i].position.z);
            pos = transform.position + transform.rotation * pos * tileSize;

            Gizmos.DrawMesh(monsterSpawnMesh, -1, pos, rot, tileDimensions / 4.0f);
        }

        Gizmos.color = Color.yellow;
        Quaternion rotation = Quaternion.Euler(playerSpawnPoint.rotation) * transform.rotation;
        pos = new Vector3(playerSpawnPoint.position.x, 
            playerSpawnPoint.position.y, 
            playerSpawnPoint.position.z);
        pos = transform.position + transform.rotation * pos * tileSize;

        Gizmos.DrawMesh(monsterSpawnMesh, -1, pos, rotation, tileDimensions / 4.0f);
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

    // Spawns monsters in the room.
    public void SpawnMonsters(Vector3 roomPosition, float angle) {
        if (monsterPool == null ||
            monsterPool.monsters.Count == 0 || 
            monsterSpawnPoints.Count == 0)
            return;

        Quaternion pivotRot = Quaternion.AngleAxis(angle,
            new Vector3(0.0f, 1.0f, 0.0f));

        int monsterAmount = Random.Range(minMonsters, maxMonsters + 1);

        for (int i = 0; i < monsterAmount; i++) {
            int pointIndex = Random.Range(0, monsterSpawnPoints.Count);
            int monIndex = Random.Range(0, monsterPool.monsters.Count);
            MonsterSpawnPoint spawnPoint = monsterSpawnPoints[pointIndex];
            
            Vector3 spawnPos = (pivotRot * spawnPoint.position * tileSize) + roomPosition;
            Vector3 spawnRot = new Vector3(spawnPoint.rotation.x, 
                spawnPoint.rotation.y + angle, spawnPoint.rotation.z);

            Object.Instantiate(monsterPool.monsters[monIndex], spawnPos,
                Quaternion.Euler(spawnRot), monstersParent.transform);
        }
    }

    // Spawns the player in the room.
    public void SpawnPlayer(GameObject player, Vector3 roomPosition, float angle) {
        Quaternion pivotRot = Quaternion.AngleAxis(angle,
            new Vector3(0.0f, 1.0f, 0.0f));
            
        Vector3 spawnPos = (pivotRot * playerSpawnPoint.position * tileSize) + roomPosition;
        Vector3 spawnRot = new Vector3(playerSpawnPoint.rotation.x, 
            playerSpawnPoint.rotation.y + angle, playerSpawnPoint.rotation.z);

        GameObject pl = Object.Instantiate(player, spawnPos, Quaternion.Euler(spawnRot));
        pl.SetActive(true);
    }

}
