using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TileList
{
    public List<TileInfo> TileInfo = new List<TileInfo>();
}
public class Room
{
    public readonly int x;
    public readonly int y;
    public readonly bool[] MyEnableDir;
    public readonly int RoomID;
    public readonly int RoomBitDir;
    public Room(int roomx, int roomy, bool[] enableDir, int roomID, int roomBitDir)
    {
        x = roomx;
        y = roomy;
        MyEnableDir = enableDir;
        RoomID = roomID;
        RoomBitDir = roomBitDir;
    }
}
public class MapGenerator
{
    public int MaxX;
    public int MaxY;
    public List<Room> Rooms = new List<Room>();
    public MapGenerator(int mapSizeX, int mapSizeY)
    {
        MaxX = mapSizeX;
        MaxY = mapSizeY;
    }

    public void RoomsGenerate()
    {

        var roomAmountLimit = new Tuple<int, int>((int)((MaxX * MaxY) * 0.6f), (int)((MaxX * MaxY) * 0.8f));
        bool[] dir = new bool[4]; // 1r 2u 3l 4d

        var startPointx = (MaxX % 2 == 0) ?
            ((MaxX) / 2) + UnityEngine.Random.Range(-1, 0) : ((MaxX) / 2) + UnityEngine.Random.Range(-1, 1);
        var startPointy = (MaxY % 2 == 0) ?
            ((MaxY) / 2) + UnityEngine.Random.Range(-1, 0) : ((MaxY) / 2) + UnityEngine.Random.Range(-1, 1);

        var roomID = 0;
        List<Tuple<int, int>> roomCreator = new List<Tuple<int, int>> // x,y
            {
                new Tuple<int, int>(startPointx, startPointy)
            };
        List<Tuple<int, int>> newRoom = new List<Tuple<int, int>>();

        bool totalLoopbreakable = false;
        while (!totalLoopbreakable)
        {
            roomID = 0;
            int maxRepeat = ((MaxX + MaxY) / 2) + UnityEngine.Random.Range(0, (MaxX + MaxY) / 2) - 2;

            startPointx = (MaxX % 2 == 0) ?
                ((MaxX) / 2) + UnityEngine.Random.Range(-1, 1) : ((MaxX) / 2) + UnityEngine.Random.Range(-1, 2);
            startPointy = (MaxY % 2 == 0) ?
                ((MaxY) / 2) + UnityEngine.Random.Range(-1, 1) : ((MaxY) / 2) + UnityEngine.Random.Range(-1, 2);

            roomCreator.Clear();
            roomCreator.Add(new Tuple<int, int>(startPointx, startPointy));
            Rooms.Clear();
            totalLoopbreakable = true;
            for (int repeat = 0; repeat < maxRepeat; repeat++)
            {
                newRoom.Clear();
                foreach (var room in roomCreator)
                {
                    dir = SetRandomDir(room.Item1, room.Item2);

                    if (dir.Count(i => i == true) > 0)
                    {
                        if (dir[0]) // r
                        {
                            if (CheckConnectableDir(room.Item1 + 1, room.Item2).Count(i => i == true) > 0 || repeat == 0)
                            {
                                newRoom.Add(new Tuple<int, int>(room.Item1 + 1, room.Item2));
                            }
                        }
                        if (dir[1]) // u
                        {
                            if (CheckConnectableDir(room.Item1, room.Item2 - 1).Count(i => i == true) > 0 || repeat == 0)
                            {
                                newRoom.Add(new Tuple<int, int>(room.Item1, room.Item2 - 1));
                            }
                        }
                        if (dir[2]) // l
                        {
                            if (CheckConnectableDir(room.Item1 - 1, room.Item2).Count(i => i == true) > 0 || repeat == 0)
                            {
                                newRoom.Add(new Tuple<int, int>(room.Item1 - 1, room.Item2));
                            }
                        }
                        if (dir[3]) // d
                        {
                            if (CheckConnectableDir(room.Item1, room.Item2 + 1).Count(i => i == true) > 0 || repeat == 0)
                            {
                                newRoom.Add(new Tuple<int, int>(room.Item1, room.Item2 + 1));
                            }
                        }
                        if (!IsRoomHere(room.Item1, room.Item2))
                        {
                            Rooms.Add(new Room(room.Item1, room.Item2, dir, roomID++, ConvertTheDirToInt(dir)));
                        }
                    }
                }
                roomCreator.Clear();
                foreach (var room in newRoom)
                {
                    roomCreator.Add(room);
                }
            }

            // Final Check 
            for (int x = 0; x < MaxX; x++)
            {
                for (int y = 0; y < MaxY; y++)
                {
                    var connectableDir = CheckConnectableDir(x, y);
                    if (!IsRoomHere(x, y) && connectableDir.Count(i => i == true) > 0)
                    {
                        Rooms.Add(new Room(x, y, connectableDir, roomID++, ConvertTheDirToInt(connectableDir)));
                    }
                }
            }
            if (Rooms.Count >= roomAmountLimit.Item1 && Rooms.Count <= roomAmountLimit.Item2)
            {
                for (int i = 0; i < MaxY; i++)
                {
                    if (ActiveRoomCount(-1, i) == 0)
                    {
                        totalLoopbreakable = false;
                        break;
                    }
                }
                for (int i = 0; i < MaxX; i++)
                {
                    if (ActiveRoomCount(i) == 0)
                    {
                        totalLoopbreakable = false;
                        break;
                    }
                }
            }
            else
            {
                totalLoopbreakable = false;
            }

        }

    }

    /// <summary>
    /// Convert Direction to 0&1 R/U/L/D 
    /// </summary>
    private int ConvertTheDirToInt(bool[] dirs)
    {
        var bitdirs = default(int);
        foreach (var dir in dirs)
        {
            bitdirs <<= 1;
            if (dir) { bitdirs |= 1; }
        }
        return int.Parse(Convert.ToString(bitdirs, 2));
    }

    /// <summary>
    /// Find connectable room and return direction num
    /// </summary>  
    public bool[] CheckConnectableDir(int currentX, int currentY)
    {
        bool[] connectableDirs = new bool[4];
        Array.Fill(connectableDirs, false);
        for (int i = 1; i >= -2; i--)
        {
            var xdelta = (int)default;
            var ydelta = (int)default;
            if (Math.Abs(i) % 2 == Math.Abs(1))
            {
                xdelta = (i == 1) ? 1 : -1;
            }
            if (Math.Abs(i) % 2 == 0)
            {
                ydelta = (i == 0) ? -1 : 1;
            }

            foreach (var room in Rooms)
            {
                if (room.x == currentX + xdelta && room.y == currentY + ydelta)
                {
                    if (xdelta == -1 && room.MyEnableDir[0])
                    {
                        connectableDirs[2] = true;
                    }
                    if (ydelta == -1 && room.MyEnableDir[3])
                    {
                        connectableDirs[1] = true;
                    }
                    if (xdelta == 1 && room.MyEnableDir[2])
                    {
                        connectableDirs[0] = true;
                    }
                    if (ydelta == 1 && room.MyEnableDir[1])
                    {
                        connectableDirs[3] = true;
                    }
                    break;
                }
            }
        }
        return connectableDirs;
    }
    /// <summary>
    /// Find direction of room can place and choose randomly if it can
    /// </summary>
    public bool[] SetRandomDir(int currentX, int currentY)
    {
        bool[] dirs = new bool[4];
        Array.Fill(dirs, false);
        for (int i = 1; i >= -2; i--)
        {
            var xdelta = (int)default;
            var ydelta = (int)default;

            if (Math.Abs(i) % 2 == Math.Abs(1))
            {
                xdelta = (i == 1) ? 1 : -1;
            }
            if (Math.Abs(i) % 2 == 0)
            {
                ydelta = (i == 0) ? -1 : 1;
            }

            if (CheckInsideMap(currentX + xdelta, currentY + ydelta))
            {
                dirs[Math.Abs(i - 1)] = !IsRoomHere(currentX + xdelta, currentY + ydelta);
            }
        }
        for (int i = 0; i < dirs.Length; i++)
        {
            if (dirs.Count(i => i == true) > 1 && dirs[i] == true)
            {
                dirs[i] = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
            }
        }

        var connectableDir = CheckConnectableDir(currentX, currentY);
        for (int i = 0; i < dirs.Length; i++)
        {
            if (connectableDir[i])
            {
                dirs[i] = connectableDir[i];
            }
        }
        return dirs;
    }

    /// <summary>
    /// Check the room position that is inside of map size
    /// </summary>
    public bool CheckInsideMap(int x, int y)
    {
        if (x >= 0 && x < MaxX && y >= 0 && y < MaxY)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check the position is already used
    /// </summary>
    public bool IsRoomHere(int nextx, int nexty)
    {
        foreach (Room room in Rooms)
        {
            if (room.x == nextx && room.y == nexty)
            {
                return true;
            }

        }
        return false;
    }
    /// <summary>
    /// Count the rooms amount of near position
    /// </summary>
    public int ActiveRoomCount(int xPos = -1, int yPos = -1)
    {
        int amount = 0;
        if (yPos == -1 && xPos == -1)
        {
            return Rooms.Count;
        }
        else if (yPos != -1)
        {
            foreach (var i in Rooms)
            {
                if (i.y == yPos)
                {
                    amount++;
                }
            }
        }
        else if (xPos != -1)
        {
            foreach (var i in Rooms)
            {
                if (i.x == xPos)
                {
                    amount++;
                }
            }
        }
        return amount;
    }
}

/// <summary>
/// this must CREATE when jump in to the game from title.
/// </summary>
public class RoomManager : MonoBehaviour
{
    private static RoomManager instance;
    private static MapGenerator mapGenerator;

    // For RoomCreate, Need to be set [Room] prefab in inspector
    [SerializeField] private IngameRoom roomObj;

    // For Effect, Need to be set [FadeCanvas] prefab in inspector, This Canvas must set at below than any canvas objects
    [SerializeField] private FadeEffectManager fadeEffectGenerator;

    // For Find Player, Need to be set layer [Player] in inspector
    [SerializeField] private LayerMask playerMask;

    private GameObject RoomContainer;

    private List<IngameRoom> GameRooms = new List<IngameRoom>();

    private List<TileList> roomDesigns = new List<TileList>();
    private GameObject playerObj;
    public GameObject PlayerObj { get => playerObj; }

    [SerializeField] private UIWorldmap uiWorldmap;
    [SerializeField] private MainCameraController mainCameraController;


    public readonly int mapSizeX = 5;
    public readonly int mapSizeY = 5;

    public static RoomManager Instance
    {
        get => instance;
    }
    private void Awake()
    {
        instance = this;
        mapGenerator = new MapGenerator(mapSizeX, mapSizeY);
        TileDataParsing();
        var roomManagerCount = FindObjectsByType <RoomManager> (FindObjectsSortMode.None);
        if (roomManagerCount.Length > 1)
        {
            Destroy(roomManagerCount[roomManagerCount.Length - 1].gameObject);
        }

        mapGenerator.RoomsGenerate();
        FindAndGetPlayerObjWithLayer();

        RoomContainer = new GameObject("RoomContainer");
        MapGenerate();
        PlayerObj.transform.position = GameRooms[0].transform.localPosition;
    }

    private void TileDataParsing()
    {
        var filenum = 0;
        while(true) 
        {
            var fileLocation = $"{Application.persistentDataPath}/MapSave/MapData4dir{filenum++}.Json";

            FileInfo fileInfo = new FileInfo(fileLocation);
            if (!fileInfo.Exists)
            {
                break;
            }           
            roomDesigns.Add(JsonUtility.FromJson<TileList>(File.ReadAllText(fileLocation)));
        }
    }

    private void MapGenerate()
    {
        var grid = new GameObject("Grid"); grid.AddComponent<Grid>();
        grid.transform.parent = RoomContainer.transform;
        
;       int portalRoomNum = UnityEngine.Random.Range(1, mapGenerator.Rooms.Count);
        int shopRoomNum = UnityEngine.Random.Range(1, mapGenerator.Rooms.Count);
        foreach (var room in mapGenerator.Rooms)
        {
            var mapPadding = 48;
            var roomXpos = (room.x - (mapGenerator.MaxX / 2)) * mapPadding;
            var roomYpos = -(room.y - (mapGenerator.MaxY / 2)) * mapPadding;

            var obj = Instantiate(roomObj, new Vector2(roomXpos, roomYpos), transform.rotation, grid.transform);
            GameRooms.Add(obj);
            obj.GetMajorObects(uiWorldmap, mainCameraController);

            if (room.RoomID > 0)
            {
                if(portalRoomNum == room.RoomID)
                {
                    obj.Initialize(room, roomDesigns[1], GlobalEnums.RoomType.Portal);
                }
                else if(shopRoomNum == room.RoomID)
                {
                    obj.Initialize(room, roomDesigns[2], GlobalEnums.RoomType.Shop);
                }
                else
                {
                    obj.Initialize(room, GetMatchedTileList(room.MyEnableDir), GlobalEnums.RoomType.Normal);
                }
            }
            else
            {
                obj.Initialize(room, roomDesigns[0], GlobalEnums.RoomType.Start);
            }
        }
        foreach (var roomobj in GameRooms)   
        {
            for (int i = 0; i < 4; i++)
            {
                if (roomobj.MyRoomData.MyEnableDir[i])
                {
                    if (i == 0)
                    {
                        roomobj.ConnectedRoom[i] = GetRoomData(roomobj.MyRoomData.x + 1, roomobj.MyRoomData.y);
                    }
                    if (i == 1)
                    {
                        roomobj.ConnectedRoom[i] = GetRoomData(roomobj.MyRoomData.x, roomobj.MyRoomData.y - 1);
                    }
                    if (i == 2)
                    {
                        roomobj.ConnectedRoom[i] = GetRoomData(roomobj.MyRoomData.x - 1, roomobj.MyRoomData.y);
                    }
                    if (i == 3)
                    {
                        roomobj.ConnectedRoom[i] = GetRoomData(roomobj.MyRoomData.x, roomobj.MyRoomData.y + 1);
                    }
                }
            }
        }
    }

    private TileList GetMatchedTileList(bool[] dirs) => roomDesigns.ElementAt(UnityEngine.Random.Range(3, roomDesigns.Count));

    private IngameRoom GetRoomData(int myRoomX, int myRoomY)
    {
        foreach (var targetRoom in GameRooms)
        {
            if (targetRoom.MyRoomData.x == myRoomX && targetRoom.MyRoomData.y == myRoomY)
            {
                return targetRoom;
            }
        }
        return null;
    }

    private void FindAndGetPlayerObjWithLayer()
    {
        var allGameObject = FindObjectsOfType<GameObject>();
        for(int index = 0; index < allGameObject.Length; index++)
        {
            if (allGameObject[index] != null)
            {
                if ((playerMask & (1 << allGameObject[index].layer)) != 0)
                {
                    playerObj = allGameObject[index];
                    break;
                }
            }
        }
    }

    public void MoveToPosition(GameObject targetObject, Vector3 positionToMove) => targetObject.transform.position = positionToMove;

    public bool IsPlayerInRoom(int roomID)
    {
        var pos = PlayerObj.transform.position;
        var roompos = GameRooms[roomID].transform.position;
        return pos.y > roompos.y - 24 && pos.y < roompos.y + 24 & pos.x > roompos.x - 24 && pos.x < roompos.x + 24;
    }

    public void ActivateRoom(int targetRoomID)
    {
        GameRooms[targetRoomID].ChangeStatus(GlobalEnums.RoomFSMstatus.Active);
    }
    public void DeactivateRoom(int targetRoomID)
    {
        GameRooms[targetRoomID].ChangeStatus(GlobalEnums.RoomFSMstatus.Inactive);
    }

    public IngameRoom[] GetRoomData() => GameRooms.ToArray();

    public IngameRoom GetRoomData(int roomNum = - 1) => GameRooms[roomNum];

    public IngameRoom GetActiveRoom()
    {
        var room = GameRooms.Where(i => i.isActiveAndEnabled && IsPlayerInRoom(i.MyRoomData.RoomID));
        return (room.Count() > 0) ? room.First() : null;
    }
}
