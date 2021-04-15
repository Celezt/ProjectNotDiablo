//--------------------------------------------------------------------------//
//  DungeonGenerator.cs
//  By: Kasper S. Skott
//--------------------------------------------------------------------------//
/*
    ----- HOW IT WORKS -----
    Updated: 2021-04-15

    1.  Randomly picked rooms are placed somewhat randomly, but relatively 
        near each other. This is because we want the rooms to collide somewhat
        for the next step. If we cluster the rooms, then the next step will
        displace them outwards, and the layout will solve itself with minimal
        gaps between most rooms.

    2.  Collision detection of each room is checked, and if they're colliding,
        displace the colliding rooms. This step is repeated until there's
        no more collision between the rooms.

    3.  Each room has at least one connection. A connection is a tile that
        indicates an entrance or exit of a room. Connections of rooms need to
        be connected in order to navigate between rooms.
            Each connection must find its target connection. A connection
        will prefer the nearest connection that isn't attached to the same
        room. The target connection must also not be attached to a room that
        the room of the current connection is already connected to. If this,
        for some reason, fails (which it really shouldn't), it goes ahead and
        picks the nearest room regardless.

    4.  [Not implemented] Now that each connection has a target connection,
        we can build corridors! From each connection it will run a
        path-finding algorithm to its target, with rooms as obstacles. The
        finished path will be marked in a grid, which all connections share
        access to.

    5.  [Not implemented] After all corridor tiles have been identified, it
        needs to be cleaned up. Corridors that run in parallel must be 
        removed.

    6.  [Not implemented] The tile grid is read in order to determine which
        corridor tile to place on each tile. This is inferred by its 
        surrounding tiles in a 3x3 pattern.

    7.  [Not implemented] The dungeon layout is complete. The rooms may now 
        spawn their content.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Represents a tile that needs to be connected.
// Each doorway in a room must have a connection specified.
public struct Connection
{
    public Vector2Int position; // Should be the tile that's on the outside of the room
    public int room;            // Index of the room it originates from
    public bool connected;
}

public struct Room
{
    public GameObject roomObject;
    public RectInt bounds;
    public List<int> connections;

    public void Move(int x, int y) 
    {
        roomObject.transform.Translate(x, 0.0f, y);
        bounds.x = bounds.x + x;
        bounds.y = bounds.y + y;
    }
}

public class DungeonGenerator : MonoBehaviour
{

    public GameObject tile_1x1;

    [Range(1, 32)]
    public int minRooms = 10;
    
    [Range(1, 32)]
    public int maxRooms = 12;

    [Tooltip("The maximum number of attempts at resolving room collision "+
        "when creating a dungeon. If it exceeds this number, it will start "+
        "over with a new dungeon, scrapping the current one.")]
    [Range(20, 100)]
    public int maxRoomCollisions = 50;
    
    [Header("Debug")]
    public bool visualize = true;

    [Tooltip("Toggle between visualizing the graph of corridor connections "+
        "and graph of rooms that are connected.")]
    public bool lineToggle = true;

    //----------------------------------------------------------------------//

    private List<Room> rooms;

    private int roomResolveCount;

    private List<Connection> connections;   // Act as corridor graph vertices
    private List<int> connectionEdges;      // Act as corridor graph edges (1:1)
    private List<List<int>> connectedRooms; // Lookup table for which rooms are connected to which

    //----------------------------------------------------------------------//

    void GenerateRoom(Vector2Int position)
    {
        // Generate randomly sized rooms with random connections for now.
        // TODO: Pick a random premade room prefab instead
        int roomWidth = Random.Range(7, 10);
        int roomHeight = Random.Range(7, 10);
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
        room.connections = new List<int>();

        // Set room to have 1 extra tile of padding, so that rooms must have
        // at least 1 empty tile between them.
        room.bounds = new RectInt((int)position.x - 1, (int)position.y - 1, roomWidth + 2, roomHeight + 2);

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
                conn.room = rooms.Count;

                connections.Add(conn);
                room.connections.Add(connections.Count - 1); // Index of the last added connection
                Debug.Log("Created connection at " + connPos);
            }
        }

        rooms.Add(room);
        connectedRooms.Add(new List<int>());

        Debug.Log("Created room at " + room.bounds);
    }

    void MoveRoom(int roomIndex, int x, int y)
    {
        Room tmp = rooms[roomIndex];
        tmp.Move(x, y);
        rooms[roomIndex] = tmp;
        
        // Move connections
        List<int> connList = rooms[roomIndex].connections;
        for (int i = 0; i < connList.Count; i++) {
            Vector2Int newPos = new Vector2Int(x, y);
            newPos += connections[connList[i]].position;

            Connection tmpConn = connections[connList[i]];
            tmpConn.position = newPos;
            connections[connList[i]] = tmpConn;
        }
    }

    // This should be called until it returns false, i.e. no collisions ocurred.
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
                        
                        Vector2 overlap = new Vector2((r1.width / 2.0f + r2.width / 2.0f) - (r2.center.x - r1.center.x),
                            (r1.height / 2.0f + r2.height / 2.0f) - (r2.center.y - r1.center.y));
                        
                        int randMove = Random.Range(0, 10); // Introduce some randomness
                        if (randMove == 0) { // Move x
                            MoveRoom(i, -1, 0);
                            MoveRoom(j, 1, 0);
                        }
                        else if (randMove == 1) { // Move y
                            MoveRoom(i, 0, -1);
                            MoveRoom(j, 0, 1);
                        }
                        else {
                            if (Mathf.Abs(overlap.x) >= Mathf.Abs(overlap.y)) {
                                MoveRoom(i, (int)(-overlap.x / 2.0f), 0);
                                MoveRoom(j, (int)(overlap.x / 2.0f), 0);
                            }
                            else {
                                MoveRoom(i, 0, (int)(-overlap.y / 2.0f));
                                MoveRoom(j, 0, (int)(overlap.y / 2.0f));
                            }
                        }
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

        for (int i = 0; i < connectedRooms.Count; i++)
            connectedRooms[i].Clear();
        
        connectedRooms.Clear();
    }

    // Returns true if dungeon was created successfully, otherwise false.
    bool GenerateDungeon()
    {
        int roomCount = Random.Range(minRooms, maxRooms + 1);
        for (int i = 0; i < roomCount; i++) {
            GenerateRoom(new Vector2Int(Random.Range(0, 17), Random.Range(0, 17)));
        }
        
        while (ResolveRoomCollision()) {
            if (roomResolveCount > maxRoomCollisions) {
                Debug.Log("Room collision resolution went through too many iterations "+
                    "(> "+maxRoomCollisions+"). Aborting...");

                DestroyDungeon();
                return false;
            }
        }

        CreateCorridorGraph();

        return true;
    }

    //----------------------------------------------------------------------//
    // CORRIDORS
    //----------------------------------------------------------------------//
    
    // Returns the index into the "connections" member, where the nearest
    // connection is. Connections that are farther away than maxDistance
    // are ignored.
    // -----
    // If anyRoom is false, only rooms that are not yet connected to that room
    // is connected to. If anyRoom is true, any room can be connected to, 
    // except the room where "from" exists.
    int GetNearestConnection(int from, bool anyRoom)
    {
        float tmpDist;
        int minIndex = -1;
        float minDist = float.MaxValue;
        int jRoom;
        int fromRoom;

        for (int j = 0; j < connections.Count; j++) {
            jRoom = connections[j].room;
            fromRoom = connections[from].room;

            // If not the same room and (room is not already connected or anyRoom)
            if (fromRoom != jRoom &&                                  
               (!connectedRooms[fromRoom].Contains(jRoom) || anyRoom)
            ) {
                tmpDist = Vector2Int.Distance(connections[j].position,
                    connections[from].position);

                if (tmpDist < minDist)
                {
                    minIndex = j;
                    minDist = tmpDist;
                }
            }
        }

        return minIndex;
    }

    void CreateCorridorGraph()
    {
        connectionEdges.Clear();

        // For each connection, find the nearest connection of another room.
        for (int i = 0; i < connections.Count; i++) {

            // First, only try to find the nearest connection to rooms that
            // the current room isn't already connected to.
            int nearest = GetNearestConnection(i, false);

            // If no connection was found within maxConnectionDist that the 
            // current room wasn't already connected to; pick any room.
            if (nearest == -1)
                nearest = GetNearestConnection(i, true);

            connectionEdges.Add(nearest); // Associate connection[i] with the nearest connection

            int currRoom = connections[i].room;
            // Add to the lookup table that rooms[i] is connected to the
            // room of the connection picked.
            if (!connectedRooms[currRoom].Contains(connections[nearest].room)) {
                connectedRooms[currRoom].Add(connections[nearest].room);
                connectedRooms[connections[nearest].room].Add(currRoom);
            }

        }
    }

    // TODO
    // Steps through one tile in the corridor creation from the given 
    // connection.
    // Returns true if the corridor stopped/finished
    bool CorridorCreationStep(Connection from, Connection to)
    {

        return true;
    }

    //----------------------------------------------------------------------//

    DungeonGenerator()
    {
        rooms = new List<Room>();
        connections = new List<Connection>();
        connectionEdges = new List<int>();
        connectedRooms = new List<List<int>>();
        roomResolveCount = 0;
    }

    void Start()
    {
        // Continue trying to generate a new dungeon if room collision 
        // iterations exceed the limit.
        while (!GenerateDungeon());

        Debug.Log("Dungeon created.");
    }

    void Update()
    {
        if (Keyboard.current.dKey.wasReleasedThisFrame) {
            DestroyDungeon();
            double time = Time.timeAsDouble*1000.0;
            while (!GenerateDungeon());
            Debug.Log("Dungeon generation took: " + (Time.timeAsDouble*1000.0 - time) + "ms");
        }

        // DEBUG: For manually stepping through room collision resolution.
        /*
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

        // DEBUG: For manually stepping through corridor creation
        /*
        if (Keyboard.current.cKey.wasReleasedThisFrame) {
            if (CorridorCreationStep(corridorIt)) {
                Debug.Log("Corridor stopped.");
            }
        }
        */

        if (visualize) {
            if (lineToggle) {
                for (int i = 0; i < connectionEdges.Count; i++) {
                    if (connectionEdges[i] == -1)
                        continue;

                    Connection connStart = connections[i];
                    Connection connEnd = connections[connectionEdges[i]];
                    Vector3 start = new Vector3(connStart.position.x + 0.5f, 0.0f, 
                        connStart.position.y + 0.5f);
                    Vector3 end = new Vector3(connEnd.position.x + 0.5f, 0.0f, 
                        connEnd.position.y + 0.5f);
                    Debug.DrawLine(start, end, Color.magenta);
                }
            }
            else {
                for (int i = 0; i < rooms.Count; i++) {
                    for (int j = 0; j < connectedRooms[i].Count; j++) {       
                        Vector3 start = rooms[i].roomObject.transform.position;
                        Vector3 end = rooms[connectedRooms[i][j]].roomObject.transform.position;
                        Debug.DrawLine(start, end, Color.red);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (visualize) {
            // Draw rooms
            for (int i = 0; i < rooms.Count; i++) {     
                Gizmos.color = Color.HSVToRGB(i*0.05f, 0.8f, 0.8f);
                Gizmos.DrawCube(rooms[i].roomObject.transform.position, 
                    new Vector3(rooms[i].bounds.width - 2, 0.5f + i * 0.02f, rooms[i].bounds.height - 2));
            }

            Gizmos.color = Color.green;
            for (int i = 0; i < connections.Count; i++) {
                Vector3 tilePos = new Vector3(connections[i].position.x + 0.5f, 0.0f, 
                    connections[i].position.y + 0.5f);
                Gizmos.DrawCube(tilePos, new Vector3(1.0f, 1.0f, 1.0f));
            }
        }
    }

}
