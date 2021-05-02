//--------------------------------------------------------------------------//
//  DungeonGenerator.cs
//  By: Kasper S. Skott
//--------------------------------------------------------------------------//
/*
    ----- HOW IT WORKS -----
    Updated: 2021-04-20

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
        destroyed, and restarts from step 1.

    6.  [Not implemented] The tile grid is read in order to determine which
        corridor tile to place on each tile. This is inferred by its 
        surrounding tiles in a 3x3 grid.

    7.  [Not implemented] The dungeon layout is complete. The rooms may now 
        spawn their content.

*/
//--------------------------------------------------------------------------//

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

using Debug = UnityEngine.Debug;

public class DungeonGenerator : MonoBehaviour
{
    //----------------------------------------------------------------------//
    // Utility structs and enums
    //----------------------------------------------------------------------//

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
            roomObject.transform.Translate(x, 0.0f, y);
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
        for (int i = 0; i < rooms.Count; i++)
            Object.Destroy(rooms[i].roomObject);

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
    bool GenerateDungeon()
    {
        int roomCount = Random.Range(minRooms, maxRooms + 1);
        for (int i = 0; i < roomCount; i++) {
            GenerateRoom(new Vector2Int(Random.Range(0, 17), Random.Range(0, 17)));
        }
        
        while (ResolveRoomCollision()) {
            if (roomResolveCount > maxRoomCollisions) {
                Debug.Log("Room collision resolution went through too many iterations "+
                    "(> "+maxRoomCollisions+"). Destroying dungeon.");

                DestroyDungeon();
                return false;
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

        for (int y = 0; y < tileGridSize.y; y++)
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

        corridorStepIt = 0;
        tileGridSize = new Vector2Int();
        tileGridOffset = new Vector2Int();
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
        // DEBUG: Regenerate dungeon
        if (Keyboard.current.dKey.wasReleasedThisFrame) {
            DestroyDungeon();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!GenerateDungeon());
            
            stopwatch.Stop();
            Debug.Log("Dungeon generation took: " + stopwatch.ElapsedMilliseconds + "ms");
        }

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
            Debug.DrawLine(new Vector3(0.0f, 0.0f, 0.0f) + gridOrigin, 
                new Vector3(tileGridSize.x, 0.0f, 0.0f) + gridOrigin, 
                Color.green);
            Debug.DrawLine(new Vector3(tileGridSize.x, 0.0f, 0.0f) + gridOrigin,
                new Vector3(tileGridSize.x, 0.0f, tileGridSize.y) + gridOrigin,
                Color.green);
            Debug.DrawLine(new Vector3(tileGridSize.x, 0.0f, tileGridSize.y) + gridOrigin,
                new Vector3(0.0f, 0.0f, tileGridSize.y) + gridOrigin,
                Color.green);
            Debug.DrawLine(new Vector3(0.0f, 0.0f, tileGridSize.y) + gridOrigin,
                new Vector3(0.0f, 0.0f, 0.0f) + gridOrigin,
                Color.green);
        }
    }

    void OnDrawGizmos()
    {
        if (showTileGrid) {
            for (int x = 0; x < tileGridSize.x; x++) {
                for (int y = 0; y < tileGridSize.y; y++) {
                    Vector3 pos = new Vector3(x + 0.5f + tileGridOffset.x, 
                        0.5f, y + 0.5f + tileGridOffset.y);

                    if (tileGrid[x][y] == Tile.ROOM) {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(pos, new Vector3 (1.0f, 1.0f, 1.0f));
                    }
                    else if (tileGrid[x][y] == Tile.CORRIDOR) {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(pos, new Vector3 (1.0f, 1.0f, 1.0f));
                    }
                    else if (tileGrid[x][y] == Tile.CONNECTION) {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(pos, new Vector3 (1.0f, 1.0f, 1.0f));
                    }
                }
            }
        }
    }

}
