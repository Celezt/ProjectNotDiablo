using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct Room
{
    public GameObject roomObject;
    public RectInt bounds;

    public void Move(int x, int y) 
    {
        roomObject.transform.Translate(x, 0.0f, y);
        bounds.x = bounds.x + x;
        bounds.y = bounds.y + y;
    }
}

// Represents a tile that needs to be connected.
// Each doorway in a room must have a connection specified.
public struct Connection
{
    public Vector2Int position;    // Should be the tile that's on the outside of the room
    public bool connected;
}

public class DungeonGenerator : MonoBehaviour
{

    public GameObject tile_1x1;

    //----------------------------------------------------------------------//

    private const int ROOM_RESOLUTION_ATTEMPTS = 20;

    private List<Room> rooms;
    private List<Connection> connections;

    private int roomResolveCount;
    private bool generationSuccess;

    //----------------------------------------------------------------------//

    void GenerateRoom(Vector2Int position)
    {
        // Generate randomly sized rooms with random connections for now.
        // TODO: Pick a random premade room prefab instead
        int roomWidth = Random.Range(2, 6);
        int roomHeight = Random.Range(2, 6);
        Vector3 roomCenter = new Vector3(roomWidth / 2.0f, 0.0f, roomHeight / 2.0f);
        roomCenter.x += position.x;
        roomCenter.z += position.y;
        
        // Create 1x1 plane and scale it for now.
        // TODO: Instantiate room prefab
        GameObject roomObject = Object.Instantiate(tile_1x1, roomCenter, Quaternion.identity, transform);
        Vector3 scale = new Vector3(roomWidth, 1.0f, roomHeight);     
        roomObject.transform.localScale = scale * 0.1f; // 0.1f temporary magic number for scaling a default plane
        roomObject.name = "Room " + rooms.Count;

        // Create and add a new Room struct to the list.
        Room room = new Room();
        room.roomObject = roomObject;

        // Set room to have 1 extra tile of padding, so that rooms must have
        // at least 1 empty tile between them.
        room.bounds = new RectInt((int)position.x - 1, (int)position.y - 1, roomWidth + 2, roomHeight + 2);

        rooms.Add(room);
        Debug.Log("Created room at " + room.bounds);

        // Randomly generate connections for the room.
        // TODO: Use predetermined connections, set in the prefab.
        for (int i = Random.Range(1, 4); i > 0; i--) {

            // Pick a random tile outside the edges of the room
            Vector2Int connPos = new Vector2Int(0, Random.Range(-1, roomHeight+1));
            if (connPos.y == -1 || connPos.y == roomHeight)
                connPos.x = Random.Range(0, roomWidth);
            else
                connPos.x = Random.Range(0, 2) * (roomWidth+1) - 1;

            connPos.x += (int)position.x;
            connPos.y += (int)position.y;

            if (!connections.Exists(e => e.position.Equals(connPos))) {
                Connection conn = new Connection();
                conn.position = connPos;
                conn.connected = false;

                connections.Add(conn);
                Debug.Log("Created connection at " + connPos);
            }
        }
    }

    bool ResolveRoomCollision()
    {
        bool collision = false; // Whether a collision occured somewhere

        for (int i = 0; i < rooms.Count; i++) {
            
            // Check collision with every other room
            for (int j = 0; j < rooms.Count; j++) {
                if (j != i) {
                    RectInt r1 = rooms[i].bounds;
                    RectInt r2 = rooms[j].bounds;

                    // Do AABB collision detection
                    if (r2.xMin < r1.xMax && r2.xMax > r1.xMin &&
                        r2.yMin < r1.yMax && r2.yMax > r1.yMin) 
                    {
                        collision = true;
                        //Debug.Log(i + ":" + r1 + " is colliding with " + j + ":" + r2);
                        
                        Vector2 overlap = new Vector2((r1.width / 2.0f + r2.width / 2.0f) - (r2.center.x - r1.center.x),
                            (r1.height / 2.0f + r2.height / 2.0f) - (r2.center.y - r1.center.y));
                        
                        Room tmp1 = rooms[i];
                        Room tmp2 = rooms[j];
                        int randMove = Random.Range(0, 10); // Introduce some randomness
                        if (randMove == 0) { // Move x
                            tmp1.Move(-1, 0);
                            tmp2.Move(1, 0);
                        }
                        else if (randMove == 1) { // Move y
                            tmp1.Move(0, -1);
                            tmp2.Move(0, 1);
                        }
                        else {
                            if (Mathf.Abs(overlap.x) >= Mathf.Abs(overlap.y)) {
                                tmp1.Move((int)(-overlap.x / 2.0f), 0);
                                tmp2.Move((int)(overlap.x / 2.0f), 0);
                            }
                            else {
                                tmp1.Move(0, (int)(-overlap.y / 2.0f));
                                tmp2.Move(0, (int)(overlap.y / 2.0f));
                            }
                        }

                        rooms[i] = tmp1;
                        rooms[j] = tmp2;
                        
                        //Debug.Log("Move " + i + " by " + -overlap / 2.0f);
                        //Debug.Log("Move " + j + " by " + overlap / 2.0f);
                    }
                }
            }      
        }

        if (collision)
            roomResolveCount++;

        return collision;
    }

    void DestroyDungeon()
    {
        for (int i = 0; i < rooms.Count; i++)
            Object.Destroy(rooms[i].roomObject);

        rooms.Clear();
        connections.Clear();

        roomResolveCount = 0;
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < 12; i++) {
            GenerateRoom(new Vector2Int(Random.Range(0, 17), Random.Range(0, 17)));
        }

        generationSuccess = true;
        
        while (ResolveRoomCollision()) {
            if (roomResolveCount > ROOM_RESOLUTION_ATTEMPTS) {
                Debug.Log("Room collision resolution went through too many iterations "+
                    "(> "+ROOM_RESOLUTION_ATTEMPTS+"). Aborting...");

                DestroyDungeon();
                generationSuccess = false;
                break;
            }
        }
    }

    //----------------------------------------------------------------------//

    DungeonGenerator()
    {
        connections = new List<Connection>();
        rooms = new List<Room>();
        roomResolveCount = 0;
        generationSuccess = false;
    }

    void Start()
    {
        // If generation failed (too many collision resolve attempts),
        // try generating a new dungeon.
        while (!generationSuccess) {
            GenerateDungeon();
        }

        Debug.Log("Dungeon created.");
    }

    void Update()
    {
        /* For manually stepping through room collision resolution.

        if (Keyboard.current.sKey.wasReleasedThisFrame) {
            if (ResolveRoomCollision()) {
                Debug.Log("Room resolve iteration: " + roomResolveCount);

                for (int i = 0; i < rooms.Count; i++)
                    Debug.Log("Room " + i + ": " + rooms[i].bounds);
            }
            else {
                Debug.Log("Room collision resolved at iteration " + roomResolveCount);
            }
        }
        */
    }

    void OnDrawGizmos()
    {
        // Draw rooms
        for (int i = 0; i < rooms.Count; i++) {     
            Gizmos.color = Color.HSVToRGB(i*0.05f, 0.8f, 0.8f);
            Gizmos.DrawCube(rooms[i].roomObject.transform.position, 
                new Vector3(rooms[i].bounds.width - 2, 0.5f + i * 0.02f, rooms[i].bounds.height - 2));
        }

        /* Connections aren't fully implemented yet

        Gizmos.color = Color.green;
        for (int i = 0; i < connections.Count; i++) {
            Vector3 tilePos = new Vector3(connections[i].position.x + 0.5f, 0.0f, 
                connections[i].position.y + 0.5f);
            Gizmos.DrawCube(tilePos, new Vector3(1.0f, 1.0f, 1.0f));
        }
        */
    }

    //----------------------------------------------------------------------//
    //  INITIAL ATTEMPT -- for reference
    //----------------------------------------------------------------------//

    // public GameObject startObject; // Is placed at the start of the dungeon, always 0,0,0
    // public GameObject endObject; // Is placed at the end of the dungeon.

    // public GameObject tile_1x1;

    // [Tooltip("In units")]
    // public int tileSize = 10;

    // [Range(0.0f, 0.35f)]
    // public float flowDirectionChaos = 0.25f;

    // //----------------------------------------------------------------------//

    // private Vector3 start;
    // private Vector3 end;
    
    // private List<Vector2Int> flowPath;
    // private Vector2Int midPos;
    // private Vector2Int endPos;

    // //----------------------------------------------------------------------//

    // void GenerateFlowPath(Vector2Int from, Vector2Int to)
    // {
    //     Vector2Int currPos = from;
    //     Vector2Int lastPos = currPos;
    //     Vector2 currDir = new Vector2();
    //     int moveDecision;

    //     flowPath.Add(currPos);

    //     while (currPos != to) {
    //         moveDecision = Random.Range(0, 2);

    //         currDir.x = to.x - currPos.x;
    //         currDir.y = to.y - currPos.y;
    //         currDir.Normalize();
    //         currDir.x += Random.Range(-flowDirectionChaos, flowDirectionChaos);
    //         currDir.y += Random.Range(-flowDirectionChaos, flowDirectionChaos);
            
    //         if (Mathf.Abs(currDir.x) > Mathf.Abs(currDir.y))
    //             currPos.x += currDir.x < 0.0f ? -1 : 1;
    //         else
    //             currPos.y += currDir.y < 0.0f ? -1 : 1;

    //         lastPos = currPos;
    //         flowPath.Add(currPos);
    //     }
    // }

    // //----------------------------------------------------------------------//

    // void Start()
    // {
    //     start = new Vector3(0.0f, 0.0f, 0.0f);

    //     // Randomize position of the mid and end objects
    //     // Pick a random direction for end point
    //     Vector2 dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    //     dir.Normalize();

    //     // Pick a random distance from start
    //     float mag = Random.Range(25.0f, 30.0f);
        
    //     endPos = new Vector2Int((int)(dir.x * mag), (int)(dir.y * mag));
    //     end = new Vector3(endPos.x * tileSize, 0.0f, endPos.y * tileSize);

    //     // Adjust direction to give variation to the path
    //     float dirRand = Random.Range(0.3f, 0.5f);
    //     dir.x += Random.Range(0, 2) == 0 ? dirRand : -dirRand;

    //     dirRand = Random.Range(0.3f, 0.5f);
    //     dir.y += Random.Range(0, 2) == 0 ? dirRand : -dirRand;

    //     dir.Normalize();

    //     // Pick a random distance from start
    //     mag = Random.Range(8.0f, 17.0f);
        
    //     midPos = new Vector2Int((int)(dir.x * mag), (int)(dir.y * mag));

    //     startObject.transform.position = start;
    //     endObject.transform.position = end;

    //     // Generate a path that flows through the dungeon, from start to end.

    //     flowPath = new List<Vector2Int>(20); // 20 is just an estimate to reduce potential resizes.

    //     GenerateFlowPath(new Vector2Int(0, 0), midPos);
    //     GenerateFlowPath(flowPath[flowPath.Count-1], endPos);
        
    //     for (int i = 0; i < flowPath.Count; i++) {
    //         Vector3 tilePos = new Vector3(flowPath[i].x, 0.0f, flowPath[i].y);
    //         Object.Instantiate(tile_1x1, tilePos * tileSize, Quaternion.identity);
    //     }

    // }

    // void Update()
    // {
    //     // Visualize the dungeon flow path
    //     Vector3 lineStart = new Vector3();
    //     Vector3 lineEnd = new Vector3();
    //     for (int i = 1; i < flowPath.Count; i++) {
    //         lineStart.x = flowPath[i-1].x;
    //         lineStart.z = flowPath[i-1].y;
    //         lineEnd.x = flowPath[i].x;
    //         lineEnd.z = flowPath[i].y;
    //         Debug.DrawLine(lineStart, lineEnd, Color.green);
    //     }
    // }

    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawSphere(new Vector3(midPos.x, 0.0f, midPos.y), 1.0f);
    // }
}
