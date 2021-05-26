//--------------------------------------------------------------------------//
//  DungeonGenerator.cs
//  By: Kasper S. Skott
//--------------------------------------------------------------------------//
/*
    ----- HOW IT WORKS -----
    Updated: 2021-05-18

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

    4.  Now that each connection has a target connection, we can prepare the
        grid used to build corridors. Dynamically create a grid whose size is
        based on the layout of the rooms. Mark tiles that rooms overlap as
        room tiles, to make corridors avoid rooms. Also mark tiles that are
        connections.

    4.  Now that the tile grid is created, we can start building the corridor 
        layout (in tiles)! From each connection it will run a path-finding 
        algorithm to its target, located on the tile grid. The tiles of the 
        finished path are marked as corridor tiles. The path the corridors 
        take are weighted towards existing corridors, creating a more natural 
        layout.

    5.  After all corridor tiles have been marked, a check is made to ensure
        the corridor layout is valid. If it is invalid, the dungeon gets
        destroyed, and restarts from step 1. The weighting explained in step 4
        attempts to minimize invalid layouts.

    6.  The tile grid is read in order to determine which corridor tile, with
        which rotation to place on each tile. This is inferred by its 
        neighboring tiles.

    7.  The dungeon layout is complete. The rooms may now spawn their content.

*/
//--------------------------------------------------------------------------//

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class DungeonGenerator : MonoBehaviour
{
    //----------------------------------------------------------------------//
    // Utility structs and enums
    //----------------------------------------------------------------------//

    [System.Serializable]
    public struct TileObject
    {
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
    }

    // Represents a tile that needs to be connected.
    // Each doorway in a room must have a connection specified.
    private struct Connection
    {
        public Vector2Int position; // Should be the tile that's on the outside of the room
        public int room;            // Index of the room it originates from
        public bool connected;
    }

    private struct Room
    {
        public GameObject roomObject;
        public RectInt bounds;
        public List<int> connections;

        public void Move(int x, int y) 
        {
            bounds.x = bounds.x + x;
            bounds.y = bounds.y + y;
        }
    }

    private enum Tile : ushort
    {
        EMPTY = 0, // Default
        ROOM,
        CORRIDOR,
        CONNECTION
    }

    //----------------------------------------------------------------------//
    // Editor-exposed members
    //----------------------------------------------------------------------//

    public GameObject playerObject;

    [Min(0.0f)]
    public float tileSize = 1.0f;

    [Range(2, 100)]
    public int minRooms = 10;
    
    [Range(2, 100)]
    public int maxRooms = 12;

    [Tooltip("The maximum number of attempts at resolving room collision "+
        "when creating a dungeon. If it exceeds this number, it will start "+
        "over with a new dungeon, scrapping the current one.")]
    [Range(30, 200)]
    public int maxRoomCollisions = 50;
    
    [Header("Corridor Prefabs")]

    public TileObject corridorStraight;
    public TileObject corridorTurn;
    public TileObject corridorT;
    public TileObject corridorX;
    public TileObject corridorEnd;

    [Space(10)]
    public List<RoomPrefab> roomPrefabs;
    
    [Tooltip("The room where the dungeon ends. The player will start in the room furthest away" +
        "from this room.")]
    public RoomPrefab endRoomPrefab;

    [Header("Debug")]

    public GameObject debugTile1x1;

    [Tooltip("Visualizes the how the connections connect. An edge indicates "+
        "that the connections are directly connected; you should be able to "+
        " navigate between them.")]
    public bool showConnections = false;
  
    [Tooltip("Visualizes which rooms leads to which other rooms.")]
    public bool showRoomConnections = false;

    [Tooltip("Visualizes the tile grid. Red = room, yellow = corridor, "+
        "green = connection.")]
    public bool showTileGrid = false;

    //----------------------------------------------------------------------//
    // Rooms and layout members
    //----------------------------------------------------------------------//

    private List<Room> rooms;

    private int roomResolveCount;

    private List<Connection> connections;   // Act as corridor graph vertices
    private List<int> connectionEdges;      // Act as corridor graph edges (1:1)
    private List<List<int>> connectedRooms; // Room connection graph; which rooms can be accessed from which

    //----------------------------------------------------------------------//
    // Corridor creation members
    //----------------------------------------------------------------------//

    private int corridorStepIt;
    private Tile[][] tileGrid;          // A grid where true is a corridor tile, and false is an empty tile.
    private Vector2Int tileGridSize;    // Size of the tile grid, in tiles.
    private Vector2Int tileGridOffset;  // Position offset of the tile grid

    private int[] gCost;                // Distance to start
    private int[] hCost;                // Heuristic = distance to end
    private int[] fCost;                // g + h
    private int[] cameFrom;             // To reconstruct the path of a corridor

    //----------------------------------------------------------------------//
    // Functions
    //----------------------------------------------------------------------//

    void GeneratePrefabRoom(Vector2Int position, bool endRoom) 
    {
        float rotAngle = Random.Range(0, 4) * 90.0f;
        Quaternion randRotation = Quaternion.AngleAxis(rotAngle,
            new Vector3(0.0f, 1.0f, 0.0f));

        int roomIndex = Random.Range(0, roomPrefabs.Count);
        RoomPrefab roomPrefab = endRoom ? endRoomPrefab : roomPrefabs[roomIndex];
        Vector2Int roomBounds = roomPrefab.GetRotatedBounds(rotAngle);

        Vector3 roomCenter = new Vector3(roomBounds.x / 2.0f, 0.0f, roomBounds.y / 2.0f);
        roomCenter.x += position.x;
        roomCenter.z += position.y;

        GameObject roomObject = Object.Instantiate(roomPrefab.gameObject, roomCenter, 
            randRotation, transform);
        roomObject.name = "Room " + rooms.Count;
        roomObject.SetActive(false);

        // Create and add a new Room struct to the list.
        Room room = new Room();
        room.roomObject = roomObject;
        room.connections = new List<int>(roomPrefab.connections.Count);

        // Set room to have 1 extra tile of padding, so that rooms must have
        // at least 1 empty tile between them.
        room.bounds = new RectInt((int)position.x - 1, (int)position.y - 1, roomBounds.x + 2, roomBounds.y + 2);

        List<Vector2Int> rotatedConns = roomPrefab.GetRotatedConnections(rotAngle);
        for (int i = 0; i < rotatedConns.Count; i++) {
                Connection conn = new Connection();
                conn.position = rotatedConns[i] + position;
                conn.connected = false;
                conn.room = rooms.Count;

                connections.Add(conn);
                room.connections.Add(connections.Count - 1); // Index of the last added connection
                Debug.Log("Created connection at " + conn.position);
        }

        rooms.Add(room);
        connectedRooms.Add(new List<int>());

        Debug.Log("Created room at " + room.bounds);

    }

    // DEPRECATED
    // Generates rooms with random dimensions and connections.
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
        GameObject roomObject = Object.Instantiate(debugTile1x1, roomCenter * tileSize, Quaternion.identity, transform);
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
            else {
                i++; // Create one more connection
            }
        }

        rooms.Add(room);
        connectedRooms.Add(new List<int>());

        Debug.Log("Created room at " + room.bounds);
    }

    // Moves a room and its connections by x and y.
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
                        
                        Vector2 overlap = new Vector2(
                            (r1.width / 2.0f + r2.width / 2.0f) - (r2.center.x - r1.center.x),
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
        foreach (Transform e in transform)
            GameObject.Destroy(e.gameObject);

        rooms.Clear();
        connections.Clear();

        roomResolveCount = 0;
        corridorStepIt = 0;

        for (int i = 0; i < connectedRooms.Count; i++)
            connectedRooms[i].Clear();
        
        connectedRooms.Clear();
    }

    void InitializeTileGrid()
    {
        const int padding = 2; // Tiles per side
        int tmp;

        // First of all, find the bounds of the rooms to determine grid size

        // Find the most extreme coords between all rooms
        int minX = int.MaxValue; // Left-most
        int minY = int.MaxValue; // Bottom-most
        int maxX = int.MinValue; // Right-most
        int maxY = int.MinValue; // Top-most
        for (int i = 0; i < rooms.Count; i++) {
            tmp = rooms[i].bounds.xMin; // Left
            if (tmp < minX) minX = tmp;
            
            tmp = rooms[i].bounds.yMin; // Bottom
            if (tmp < minY) minY = tmp;

            tmp = rooms[i].bounds.xMax; // Right
            if (tmp > maxX) maxX = tmp;
            
            tmp = rooms[i].bounds.yMax; // Top
            if (tmp > maxY) maxY = tmp;
        }
                
        tileGridSize = new Vector2Int(maxX, maxY);
        tileGridSize.x -= minX;
        tileGridSize.y -= minY;

        tileGridSize.x += padding * 2; // Add padding
        tileGridSize.y += padding * 2; // Add padding

        tileGridOffset = new Vector2Int(minX - padding, minY - padding);

        // Create the tile grid
        tileGrid = new Tile[tileGridSize.x][];
        for (int x = 0; x < tileGridSize.x; x++) {
            tileGrid[x] = new Tile[tileGridSize.y];
        }

        // Create the cost arrays to be used for corridor pathfinding
        gCost = new int[tileGridSize.x * tileGridSize.y];
        hCost = new int[tileGridSize.x * tileGridSize.y];
        fCost = new int[tileGridSize.x * tileGridSize.y];
        cameFrom = new int[tileGridSize.x * tileGridSize.y];

    }

    // Mark static tiles, i.e. room tiles and connection tiles.
    void SetStaticTiles()
    {
        // Mark room tiles by iterating through all rooms and marking the
        // tiles they overlap.
        for (int i = 0; i < rooms.Count; i++) {
            RectInt bounds = rooms[i].bounds;
            
            // Convert to "grid-space"
            bounds.x -= tileGridOffset.x;
            bounds.y -= tileGridOffset.y;

            // Shrink bounds by 1 tile, because rooms intially have padding
            bounds.xMin += 1;
            bounds.yMin += 1;
            bounds.xMax -= 1;
            bounds.yMax -= 1;
            
            for (int x = bounds.xMin; x < bounds.xMax; x++) {
                for (int y = bounds.yMin; y < bounds.yMax; y++) {
                    tileGrid[x][y] = Tile.ROOM;
                }
            }
        }

        // Mark connection tiles
        for (int i = 0; i < connections.Count; i++) {
            int x = connections[i].position.x - tileGridOffset.x;
            int y = connections[i].position.y - tileGridOffset.y;
            tileGrid[x][y] = Tile.CONNECTION;
        }
    }

    // Returns true if dungeon was created successfully, otherwise false.
    public bool GenerateDungeon()
    {
        int roomCount = Random.Range(minRooms, maxRooms + 1);

        if (roomPrefabs.Count > 0) { // Generate rooms from the list of prefabs
            for (int i = 0; i < roomCount; i++) {
                Vector2Int pos = new Vector2Int(Random.Range(0, 17), Random.Range(0, 17));
                GeneratePrefabRoom(pos, i == 0); // Generate end room if it's the first room
            }
        }
        else {
            Debug.LogError("No room prefabs in DungeonGenerator!");
            return true;
        }
        
        while (ResolveRoomCollision()) {
            if (roomResolveCount > maxRoomCollisions) {
                Debug.Log("Room collision resolution went through too many iterations "+
                    "(> "+maxRoomCollisions+"). Destroying dungeon.");

                DestroyDungeon();
                return false;
            }
        }

        // Move room GameObjects to their proper position
        foreach (Room e in rooms) {
            e.roomObject.transform.position = new Vector3(e.bounds.center.x * tileSize, 
                0.0f, e.bounds.center.y * tileSize);

            RoomPrefab roomPrefab = e.roomObject.GetComponent<RoomPrefab>();    
            Vector3 rotAxis;
            float rotAngle;
            e.roomObject.transform.rotation.ToAngleAxis(out rotAngle, out rotAxis);

            List<Vector2Int> conns = roomPrefab.GetRotatedConnections(rotAngle);
            
            for (int i = 0; i < conns.Count; i++) {
                Vector3 pos = new Vector3(e.bounds.center.x, 0.0f, e.bounds.center.y);
                Connection tmp = connections[e.connections[i]];
                tmp.position = conns[i] + new Vector2Int((int)(pos.x-0.5f), (int)(pos.z-0.5f));
                connections[e.connections[i]] = tmp;
            }
        }

        CreateCorridorGraph();

        // Initilize the grid that the corridor creation algorithm uses to see
        // if a tile is blocked, and mark which tiles are corridor tiles.
        // The tiles occupied by rooms are marked as room tiles, thus
        // making them unavailable for corridor placement. Room tiles and
        // corridor tiles may be adjacent but only crossable there is a 
        // connection tile in between.
        InitializeTileGrid();

        // Mark relevant tiles as room tiles and connection tiles. These are
        // static; they won't change.
        SetStaticTiles();

        //
        while (corridorStepIt < connections.Count) {
            Vector2Int start = connections[corridorStepIt].position;
            Vector2Int end = connections[connectionEdges[corridorStepIt]].position;
            CreateCorridor(start - tileGridOffset, end - tileGridOffset);
            
            corridorStepIt++;
        }

        // Check the validity of the corridor layout. Abort dungeon generation
        // if invalid.
        if (!CheckCorridorLayout()) {
            DestroyDungeon();
            Debug.Log("Invalid corridor configuration! Destroying dungeon.");
            return false;
        }

        PlaceCorridorTiles();

        // Activate all rooms.
        foreach (Room e in rooms)
            e.roomObject.SetActive(true);

        return true;
    }

    public void BuildNavMesh()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    public void SpawnMonsters()
    {
        foreach (var e in rooms) {
            Vector3 rotAxis;
            float rotAngle;
            e.roomObject.transform.rotation.ToAngleAxis(out rotAngle, out rotAxis);

            e.roomObject.GetComponent<RoomPrefab>()
                .SpawnMonsters(e.roomObject.transform.position, rotAngle);
        }
    }

    // Spawns the player in the room that is furthest away from the end room
    public void SpawnPlayer()
    {
        int roomIndex = 0;
        float maxDist = 0.0f;

        // Find the room that is furthest from the end room
        for (int i = 1; i < rooms.Count; i++) {
            float dist = (rooms[0].bounds.center - rooms[i].bounds.center).magnitude;
            if (dist > maxDist) {
                maxDist = dist;
                roomIndex = i;
            }
        }

        Vector3 rotAxis;
        float rotAngle;
        rooms[roomIndex].roomObject.transform.rotation.ToAngleAxis(out rotAngle, out rotAxis);

        rooms[roomIndex].roomObject.GetComponent<RoomPrefab>()
            .SpawnPlayer(playerObject, rooms[roomIndex].roomObject.transform.position, rotAngle);
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

            // Add to the room connection graph that rooms[i] is connected to 
            // the room of the connection picked.
            if (!connectedRooms[currRoom].Contains(connections[nearest].room)) {
                connectedRooms[currRoom].Add(connections[nearest].room);
                connectedRooms[connections[nearest].room].Add(currRoom);
            }

        }
    }

    // Returns an index based on x, y, and the dimensions of the tile grid.
    // Returns 0 if input was out of bounds.
    int GetGridTileIndex(int x, int y)
    {
        if (x < 0 || x >= tileGridSize.x ||
            y < 0 || y >= tileGridSize.y)
            return 0;

        return x + y * tileGridSize.x;
    }

    // Returns the position of the tile based on the index, and the 
    // dimensions of the tile grid.
    // Returns {0, 0} if index is out of bounds.
    Vector2Int GetGridTileByIndex(int index)
    {
        if (index >= tileGridSize.x * tileGridSize.y)
            return new Vector2Int(tileGridSize.x-1, tileGridSize.y-1);
        
        if (index < 0)
            return new Vector2Int(0, 0);

        return new Vector2Int(index % tileGridSize.x, index / tileGridSize.x);
    }

    // Retrieves the index into the tileList that has the smallest f cost.
    int GetTileWithSmallestF(List<int> tileList)
    {
        float curr;
        float min = float.MaxValue;
        int minIndex = 0;
        for (int i = 0; i < tileList.Count; i++) {
            curr = fCost[tileList[i]];
            if (curr < min) {
                min = curr;
                minIndex = i;
            }
        }

        return minIndex;
    }

    // Use an A* algorithm to find and mark a path between the given 
    // connections as corridor tiles.
    // Returns true if a path was found, otherwise false.
    bool CreateCorridor(Vector2Int start, Vector2Int end)
    {
        int current;
        Vector2Int currPos;
        List<int> open = new List<int>(); // List of indices into cost members
        List<int> finishedPath = new List<int>();
        int finishedCount = 0;
        int startIndex = GetGridTileIndex(start.x, start.y);
        int endIndex = GetGridTileIndex(end.x, end.y);
        bool success = false;

        // Local function for calculating "Manhattan distance"
        int GetManhattanDistance(Vector2Int fromPos, Vector2Int toPos)
        {
            return Mathf.Abs(fromPos.x - toPos.x) + Mathf.Abs(fromPos.y - toPos.y) + 8;
        }

        // Local function for calculating the cost of a tile, i.e. weighting
        int GetTileCost(Vector2Int tile)
        {
            Tile t = tileGrid[tile.x][tile.y];
            return (t == Tile.CORRIDOR || t == Tile.CONNECTION) ? 2 : 8;
        }

        // Local function to determine whether a tile is valid or not
        bool IsTileValid(int x, int y) 
        {
            if (x < 0 || x >= tileGridSize.x ||
                y < 0 || y >= tileGridSize.y)
                return false;

            if (tileGrid[x][y] == Tile.ROOM) // Disallow room tiles
                return false;
            
            return true;
        }

        // Local function to get the tile that has the smallest f cost
        int GetTileWithSmallestF()
        {
            int curr;
            int min = int.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < open.Count; i++) {
                curr = fCost[open[i]];
                if (curr < min) {
                    min = curr;
                    minIndex = open[i];
                }
            }

            return minIndex;
        }

        // Local function for processing each neighbor in the inner loop
        void ProcessNeighbor(int neighbor)
        {
            Vector2Int neighborPos = GetGridTileByIndex(neighbor);
            int prelimGScore = gCost[current] + GetTileCost(neighborPos);

            // If this neighbor is a better path
             if (prelimGScore < gCost[neighbor] || prelimGScore == int.MaxValue) {
                cameFrom[neighbor] = current;

                // Update costs
                gCost[neighbor] = prelimGScore;
                fCost[neighbor] = prelimGScore + GetManhattanDistance(neighborPos, end);
                    
                if (!open.Contains(neighbor))
                    open.Add(neighbor);
            }
        }

        // Initialize the g cost of all tiles
        for (int i = 0; i < tileGridSize.x * tileGridSize.y; i++) {
            gCost[i] = int.MaxValue;
            hCost[i] = GetManhattanDistance(GetGridTileByIndex(i), end);
        }

        // Add the starting tile to the open list
        open.Add(startIndex);
        gCost[startIndex] = 0;
        hCost[startIndex] = GetManhattanDistance(start, end);
        fCost[startIndex] = hCost[startIndex];
        cameFrom[startIndex] = startIndex;

        while (open.Count > 0) {
            current = GetTileWithSmallestF(); // Get tile with least f is open
            finishedCount = finishedPath.Count;
            
            if (current == endIndex) { // The end tile was found!
                success = true;
                break;
            }

            open.Remove(current); // Remove current from the open list

            currPos = GetGridTileByIndex(current);

            // Process neighbors up, down, left, and right
            if (IsTileValid(currPos.x, currPos.y + 1)) 
                ProcessNeighbor(GetGridTileIndex(currPos.x, currPos.y + 1));

            if (IsTileValid(currPos.x, currPos.y - 1)) 
                ProcessNeighbor(GetGridTileIndex(currPos.x, currPos.y - 1));

            if (IsTileValid(currPos.x - 1, currPos.y)) 
                ProcessNeighbor(GetGridTileIndex(currPos.x - 1, currPos.y));

            if (IsTileValid(currPos.x + 1, currPos.y)) 
                ProcessNeighbor(GetGridTileIndex(currPos.x + 1, currPos.y));
        }
       
        // Mark all tiles in the finished path
        int tile = cameFrom[endIndex];
        while (tile != cameFrom[tile]) {
            Vector2Int pos = GetGridTileByIndex(tile);
            if (tileGrid[pos.x][pos.y] == Tile.EMPTY)
                tileGrid[pos.x][pos.y] = Tile.CORRIDOR;
            
            tile = cameFrom[tile];
        }

        if (success) {
            return true;
        }

        // The end was never reached. Should not happen.
        Debug.Log($"Corridor pathfinding failed: start {start}, end {end}");
        return false; 
    }

    // Checks whether the corridor layout is valid.
    // Returns false if any illegal corridor configuration was found,
    // otherwise true.
    bool CheckCorridorLayout()
    {
        bool c, e, ne, n, nw, w, sw, s, se;

        bool GetIsCorridorTile(int x, int y)
        {
            Tile tile = tileGrid[x][y];
            return tile == Tile.CORRIDOR || tile == Tile.CONNECTION;
        }

        for (int y = 0; y < tileGridSize.y; y++) {
            for (int x = 0; x < tileGridSize.x; x++) {
                c = tileGrid[x][y] == Tile.CORRIDOR;
                if (c) {
                    e   = GetIsCorridorTile(x+1, y);
                    ne  = GetIsCorridorTile(x+1, y+1);
                    n   = GetIsCorridorTile(x, y+1);
                    nw  = GetIsCorridorTile(x-1, y+1);
                    w   = GetIsCorridorTile(x-1, y);
                    sw  = GetIsCorridorTile(x-1, y-1);
                    s   = GetIsCorridorTile(x, y-1);
                    se  = GetIsCorridorTile(x+1, y-1);

                    if (ne && n && e) return false;
                    if (nw && n && w) return false;
                    if (sw && s && w) return false;
                    if (se && s && e) return false;
                }
            }
        }

        return true;
    }

    void PlaceCorridorTiles()
    {
        bool c, e, n, w, s;
        Tile center;

        bool GetIsCorridorTile(int x, int y)
        {
            Tile tile = tileGrid[x][y];

            if (center == Tile.CONNECTION) // Also count room tile if center is a connection
                return tile == Tile.CORRIDOR || tile == Tile.CONNECTION || 
                    tile == Tile.ROOM;
            else
                return tile == Tile.CORRIDOR || tile == Tile.CONNECTION;
        }

        void PlaceTile(int x, int y, TileObject tile, float rot=0.0f)
        {
            Vector3 position = new Vector3(x + tileGridOffset.x + 0.5f, 0.0f, 
                y + tileGridOffset.y + 0.5f) * tileSize;
            Quaternion rotation = Quaternion.Euler(tile.rotationOffset.x, 
                tile.rotationOffset.y + rot, tile.rotationOffset.z);

            GameObject obj = Object.Instantiate(tile.prefab, 
                position + tile.positionOffset,
                rotation, 
                transform);
            obj.SetActive(true);

        }

        for (int y = 0; y < tileGridSize.y; y++) {
            for (int x = 0; x < tileGridSize.x; x++) {
                center = tileGrid[x][y];
                c = center == Tile.CORRIDOR || center == Tile.CONNECTION;

                if (c) {
                    e = GetIsCorridorTile(x+1, y);
                    n = GetIsCorridorTile(x, y+1);
                    w = GetIsCorridorTile(x-1, y);
                    s = GetIsCorridorTile(x, y-1);

                    if (e && n && w && s) // X
                        PlaceTile(x, y, corridorX);
                    else if (e && n && s) // T
                        PlaceTile(x, y, corridorT, 0.0f);
                    else if (w && n && e) // T
                        PlaceTile(x, y, corridorT, -90.0f);
                    else if (s && w && n) // T
                        PlaceTile(x, y, corridorT, -180.0f);
                    else if (e && s && w) // T
                        PlaceTile(x, y, corridorT, -270.0f);
                    else if (e && n) // L
                        PlaceTile(x, y, corridorTurn, 0.0f);
                    else if (n && w) // L
                        PlaceTile(x, y, corridorTurn, -90.0f);
                    else if (w && s) // L
                        PlaceTile(x, y, corridorTurn, -180.0f);
                    else if (s && e) // L
                        PlaceTile(x, y, corridorTurn, -270.0f);
                    else if (e && w) // Straight
                        PlaceTile(x, y, corridorStraight, 0.0f);
                    else if (n && s) // Straight
                        PlaceTile(x, y, corridorStraight, 90.0f);
                    else if (n) // End
                        PlaceTile(x, y, corridorEnd, 0.0f);
                    else if (s) // End
                        PlaceTile(x, y, corridorStraight, 180.0f);
                    else
                        Debug.Log("Invalid corridor configuration!");
                }
            }
        }
    }

    //----------------------------------------------------------------------//

    DungeonGenerator()
    {
        rooms = new List<Room>();
        connections = new List<Connection>();
        connectionEdges = new List<int>();
        connectedRooms = new List<List<int>>();
        roomResolveCount = 0;

        corridorStepIt = 0;
        tileGridSize = new Vector2Int();
        tileGridOffset = new Vector2Int();
    }

    void Start()
    {
        // Continue trying to generate a new dungeon if room collision 
        // iterations exceed the limit.
        //while (!GenerateDungeon());

        //BuildNavMesh();
        //SpawnMonsters();
        //SpawnPlayer();
    }

    void Update()
    {
        // DEBUG: Regenerate dungeon
        // if (Keyboard.current.rKey.wasReleasedThisFrame) {
        //     DestroyDungeon();
        //     Stopwatch stopwatch = new Stopwatch();
        //     stopwatch.Start();

        //     while (!GenerateDungeon());

        //     BuildNavMesh();
        //     SpawnMonsters();

        //     stopwatch.Stop();
        //     Debug.Log("Dungeon generation took: " + stopwatch.ElapsedMilliseconds + "ms");
        // }

#if UNITY_EDITOR

        // Draw connection edges to their targets
        if (showConnections) { 
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

        // Draw room connection graph
        if (showRoomConnections) { 
            for (int i = 0; i < rooms.Count; i++) {
                for (int j = 0; j < connectedRooms[i].Count; j++) {       
                    Vector3 start = rooms[i].roomObject.transform.position;
                    Vector3 end = rooms[connectedRooms[i][j]].roomObject.transform.position;
                    Debug.DrawLine(start, end, Color.red);
                }
            }
        }

        // Draw tile grid bounds
        if (showTileGrid) { 
            Vector3 gridOrigin = new Vector3(tileGridOffset.x, 0.0f, tileGridOffset.y);
            Debug.DrawLine((new Vector3(0.0f, 0.0f, 0.0f) + gridOrigin) * tileSize, 
                (new Vector3(tileGridSize.x, 0.0f, 0.0f) + gridOrigin) * tileSize, 
                Color.green);
            Debug.DrawLine((new Vector3(tileGridSize.x, 0.0f, 0.0f) + gridOrigin) * tileSize,
                (new Vector3(tileGridSize.x, 0.0f, tileGridSize.y) + gridOrigin) * tileSize,
                Color.green);
            Debug.DrawLine((new Vector3(tileGridSize.x, 0.0f, tileGridSize.y) + gridOrigin) * tileSize,
                (new Vector3(0.0f, 0.0f, tileGridSize.y) + gridOrigin) * tileSize,
                Color.green);
            Debug.DrawLine((new Vector3(0.0f, 0.0f, tileGridSize.y) + gridOrigin) * tileSize,
                (new Vector3(0.0f, 0.0f, 0.0f) + gridOrigin) * tileSize,
                Color.green);
        }
#endif

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // foreach (Room e in rooms)
        //     Gizmos.DrawWireCube(new Vector3(e.bounds.center.x, 0.5f, e.bounds.center.y), new Vector3(e.bounds.width, 1.0f, e.bounds.height));
        
        // Gizmos.color = Color.cyan;
        // foreach (Connection e in connections)
        //     Gizmos.DrawWireCube(new Vector3(e.position.x+0.5f, 0.5f, e.position.y+0.5f), new Vector3(1.0f, 1.0f, 1.0f));

        if (showTileGrid) {
            for (int x = 0; x < tileGridSize.x; x++) {
                for (int y = 0; y < tileGridSize.y; y++) {
                    Vector3 pos = new Vector3((x + 0.5f + tileGridOffset.x ) * tileSize, 
                        tileSize / 2.0f, (y + 0.5f + tileGridOffset.y) * tileSize);

                    if (tileGrid[x][y] == Tile.ROOM) {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(pos, new Vector3 (tileSize, tileSize, tileSize));
                    }
                    else if (tileGrid[x][y] == Tile.CORRIDOR) {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(pos, new Vector3 (tileSize, tileSize, tileSize));
                    }
                    else if (tileGrid[x][y] == Tile.CONNECTION) {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(pos, new Vector3 (tileSize, tileSize, tileSize));
                    }
                }
            }
        }
    }
#endif

}
