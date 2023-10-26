using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class IngameRoom : MonoBehaviour
{
    private enum TilemapObjectID
    {
        GroundTile,
        PlatformTile,
    }

    // Tiles
    [SerializeField] private TileBase[] tileStorage;
    [SerializeField] private GameObject portalPrefab;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;

    private TileList myTileList = new TileList();
    public TileList MyTileList { get => myTileList; }
    private GameObject[] myTilemapObjects = new GameObject[2];
    public GameObject[] MyTilemapObjects { get => myTilemapObjects; }

    // RoomData
    private Room myRoomData;
    public Room MyRoomData { get => myRoomData; }
    public IngameRoom[] ConnectedRoom = new IngameRoom[4];
    private bool stagePortalCheck;
    private RoomPortal[] myPortals = new RoomPortal[4];
    private GlobalEnums.RoomType myRoomType;

    private bool isMapClear;
    public bool IsMapClear { get => isMapClear; }

    private GlobalEnums.RoomFSMstatus myFSMstatus;// = GlobalEnums.RoomFSMstatus.Inactive;

    // Objects
    [SerializeField] private GameObject chest;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject bossPortal;
    private MainCameraController mainCamera;
    private UIWorldmap uiWorldmap;

    public void Initialize(Room room, TileList tilelist, GlobalEnums.RoomType roomtype)
    {
        myRoomType = roomtype;
        myTileList = tilelist;
        myRoomData = room;
        isMapClear = (myRoomData.RoomID == 0) ? true : false;
        for (int i = 0; i < 4; i++)
        {
            if (myRoomData.MyEnableDir[i])
            {
                var createPosition = transform.position;
                var angle = (Quaternion)default;
                var portalJumpDelta = Vector3.zero;
                switch (i)
                {
                    case 0:
                        portalJumpDelta = Vector3.right;
                        angle = Quaternion.Euler(Vector3.forward * 90.0f);
                        break;
                    case 1:
                        portalJumpDelta = Vector3.up;
                        angle = Quaternion.Euler(Vector3.zero);
                        break;
                    case 2:
                        portalJumpDelta = Vector3.left;
                        angle = Quaternion.Euler(Vector3.forward * -90.0f);
                        break;
                    case 3:
                        portalJumpDelta = Vector3.down;
                        angle = Quaternion.Euler(Vector3.forward * 180.0f);
                        break;
                }
                myPortals[i] = Instantiate(portalPrefab, createPosition, angle, transform).GetComponent<RoomPortal>();
                myPortals[i].SetJumpDelta(portalJumpDelta * 2.5f);
            }
        }

        if (roomtype == GlobalEnums.RoomType.Portal)
        {
            bossPortal = Instantiate(bossPortal, transform);
            bossPortal.transform.localPosition += -(Vector3.one / 2.0f);
        }
        CreateTileObject();
        CreateEnemySpawner();
        LoadAndPlaceTiles();
        PlaceChest();
    }

    public void GetMajorObects(UIWorldmap worldmapObj, MainCameraController maincamObj)
    {
        uiWorldmap = worldmapObj;
        mainCamera = maincamObj;
    }

    public void ChangeStatus(GlobalEnums.RoomFSMstatus status)
    {
        myFSMstatus = status;
        switch (myFSMstatus)
        {
            case GlobalEnums.RoomFSMstatus.Active:
                mainCamera.SetCameraPosition(transform.position, GetMaximumRoomSizeVector(), GetMaximumRoomSizeVector(true));
                SetActiveTilemaps();
                if (enemySpawner.GetAliveEnemyCount() > 0)
                {
                    ChangeStatus(GlobalEnums.RoomFSMstatus.DoorLock);
                    break;
                }
                uiWorldmap.RefreshWorldMap();
                SetActivePortal();
                isMapClear = true;

                break;
            case GlobalEnums.RoomFSMstatus.Inactive:
                SetActivePortal(GlobalEnums.SetActive.Inactive);
                SetActiveTilemaps(GlobalEnums.SetActive.Inactive);
                SetActiveEnemySpawner(GlobalEnums.SetActive.Inactive);

                break;
            case GlobalEnums.RoomFSMstatus.DoorLock:
                SetActivePortal(GlobalEnums.SetActive.Inactive);
                SetActiveEnemySpawner();
                isMapClear = false;

                break;
        }
    }

    private void SetActiveTilemaps(GlobalEnums.SetActive isActive = GlobalEnums.SetActive.Active)
    {
        for(int i = 0; i < myTilemapObjects.Length; i++)
        {
            myTilemapObjects[i].SetActive(Convert.ToBoolean(isActive));
        }
    }

    public void SetActivePortal(GlobalEnums.SetActive isActive = GlobalEnums.SetActive.Active)
    {
        for(int i = 0; i < myPortals.Length; i++)
        {
            if (myPortals[i] != null)
            {
                myPortals[i].gameObject.SetActive(Convert.ToBoolean(isActive));
            }
        }
    }

    public void SetActiveEnemySpawner(GlobalEnums.SetActive isActive = GlobalEnums.SetActive.Active)
    {
        enemySpawner.gameObject.SetActive(Convert.ToBoolean(isActive));
    }

    private void LoadAndPlaceTiles()
    {
        var groundTileMap = myTilemapObjects[(int)TilemapObjectID.GroundTile].GetComponent<Tilemap>();
        var platformTileMap = myTilemapObjects[(int)TilemapObjectID.PlatformTile].GetComponent<Tilemap>();
        List<EnemyCreateInfo> enemyCreateInfos = new List<EnemyCreateInfo>();
        foreach (TileInfo tileinfo in myTileList.TileInfo)
        {
            TileBase targetTile = tileStorage[tileinfo.TileType];
            Vector3Int tilePos = new Vector3Int(tileinfo.TileX, tileinfo.TileY, 0);

            if (targetTile != null && groundTileMap.GetTile(tilePos) == null && platformTileMap.GetTile(tilePos) == null)
            {
                for(int i = 0; i < 4; i++)
                {
                    if(targetTile == tileStorage[i])
                    {
                        if (!myRoomData.MyEnableDir[tileinfo.TileType])
                        {
                            for (int iy = -1; iy < 2; iy++)
                            {
                                for (int ix = -1; ix < 2; ix++)
                                {
                                    var pos = tilePos + new Vector3Int(ix, iy);
                                    platformTileMap.SetTile(pos, null);
                                    groundTileMap.SetTile(pos, tileStorage[4]);
                                }
                            }
                        }
                        else
                        {
                            myPortals[tileinfo.TileType].transform.localPosition = (Vector3)tilePos + new Vector3(0.5f, 0.5f);
                            myPortals[tileinfo.TileType].transform.localScale += Vector3.right * 2.0f;
                        }
                        break;
                    }
                }

                switch (targetTile)
                {
                    case var i when (i == tileStorage[4]):
                        groundTileMap.SetTile(tilePos, targetTile);
                        continue;
                    case var i when (i == tileStorage[5]):
                        platformTileMap.SetTile(tilePos, targetTile);
                        continue;
                }

                EnemyCreateInfo enemyInfo = new EnemyCreateInfo();
                for (int i = 6; i < tileStorage.Length; i++)
                {
                    if(targetTile == tileStorage[i])
                    {
                        enemyInfo.CreatePosition = tilePos;
                        switch(i)
                        {
                            case 6: enemyInfo.CreateType = EnemyType.SmallGround; break;
                            case 7: enemyInfo.CreateType = EnemyType.SmallFly; break;
                        }
                        enemyCreateInfos.Add(enemyInfo);
                    }
                }
            }
        }
        if(enemyCreateInfos.Count > 0)
        {
            enemySpawner.Initialize(enemyCreateInfos.ToArray());
        }
    }
    private void PlaceChest()
    {
        if (UnityEngine.Random.Range(0, 2) == 0 && myRoomData.RoomID != 0 && !stagePortalCheck)
        {
            List<TileInfo> placeableTiles = FindPlaceableTile(2, 2);
            if(placeableTiles.Count > 0)
            {
                var targetTile = placeableTiles[UnityEngine.Random.Range(0, placeableTiles.Count)];
                var chestobj = Instantiate(Resources.Load<GameObject>("CJM/Prefabs/Room/Chest"), transform.position, transform.rotation);
                chestobj.transform.position += new Vector3(targetTile.TileX + 0.5f, targetTile.TileY + 1.5f);
            }
        }
    }

    private List<TileInfo> FindPlaceableTile(int objectSizeX, int objectSizeY)
    {
        List<TileInfo> resultList = new List<TileInfo>();
        var groundTilemap = myTilemapObjects[(int)TilemapObjectID.GroundTile].GetComponent<Tilemap>();
        var platformTilemap = myTilemapObjects[(int)TilemapObjectID.PlatformTile].GetComponent<Tilemap>();
        foreach (var tileinfo in myTileList.TileInfo)
        {
            var breakable = false;
            if (tileinfo.TileType == 4 && tileinfo.TileY < 19)
            {
                for (int iy = 0; iy <= objectSizeY; iy++)
                {
                    for (int ix = -objectSizeX; ix <= objectSizeX; ix++)
                    {
                        var tilepos = new Vector3Int(tileinfo.TileX + ix, tileinfo.TileY + iy);
                        if (iy == 0)
                        {
                            if (groundTilemap.GetTile(tilepos) == null || platformTilemap.GetTile(tilepos) != null)
                            {
                                breakable = true;
                                break;
                            }
                        }
                        else
                        {
                            if (groundTilemap.GetTile(tilepos) != null || platformTilemap.GetTile(tilepos) != null)
                            {
                                breakable = true;
                                break;
                            }
                        }
                    }
                    if (breakable) { break; }
                }
            }
            else
            {
                breakable = true;
            }

            if (!breakable)
            {
                resultList.Add(tileinfo);
            }
        }
        return resultList;
    }

    private void CreateTileObject()
    {
        for(int i = 0; i < myTilemapObjects.Length; i++)
        {
            string name = "";
            int layerNum = 0;
            switch(i)
            {
                case 0: 
                    name = $"[{myRoomData.RoomID}]GroundTilemap"; 
                    layerNum = (int)Mathf.Log((uint)groundLayer.value, 2);
                    break;
                case 1: 
                    name = $"[{myRoomData.RoomID}]PlatformTilemap";
                    layerNum = (int)Mathf.Log((uint)platformLayer.value, 2);
                    break;
            }

            myTilemapObjects[i] = new GameObject(name);
            myTilemapObjects[i].transform.parent = transform;
            myTilemapObjects[i].layer = layerNum;
            myTilemapObjects[i].transform.localPosition = Vector3.zero;

            myTilemapObjects[i].AddComponent<Tilemap>();
            myTilemapObjects[i].AddComponent<TilemapRenderer>();
            myTilemapObjects[i].AddComponent<TilemapCollider2D>();
            myTilemapObjects[i].AddComponent<CompositeCollider2D>();
            if(i == 1)
            {
                myTilemapObjects[1].AddComponent<PlatformEffector2D>();
                MyTilemapObjects[1].GetComponent<CompositeCollider2D>().usedByEffector = true;
            }
            myTilemapObjects[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
    }

    private void CreateEnemySpawner()
    {
        enemySpawner = Instantiate(enemySpawner, transform);
        enemySpawner.name = $"EnemySpawnerRoom[{myRoomData.RoomID}]";
    }

    private void Start()
    {
        // PortalLinkSet
        for (int i = 0; i < 4; i++)
        {
            if (myRoomData.MyEnableDir[i])
            {
                var nextNum = i + 2;
                if (nextNum > 3) { nextNum %= 4; }
                myPortals[i].Initialize(ConnectedRoom[i].myPortals[nextNum], myRoomData.RoomID);
            }
        }
        if (myRoomData.RoomID != 0)
        {
            ChangeStatus(GlobalEnums.RoomFSMstatus.Inactive);
        }
    }

    private void Update()
    {
        if(enemySpawner.GetAliveEnemyCount() < 1 && RoomManager.Instance.IsPlayerInRoom(myRoomData.RoomID) && myFSMstatus != GlobalEnums.RoomFSMstatus.Active)
        {
            isMapClear = true;
            ChangeStatus (GlobalEnums.RoomFSMstatus.Active);
        }
    }

    private Vector3 GetMaximumRoomSizeVector(bool isOpposite = false)
    {
        var maxv3 = Vector3.zero;
        var minv3 = Vector3.zero;
        foreach (var i in myTileList.TileInfo)
        {
            maxv3 = Vector3.Max(maxv3, new Vector3(i.TileX, i.TileY));
            minv3 = Vector3.Min(minv3, new Vector3(i.TileX, i.TileY));
        }
        return (isOpposite) ? minv3 : maxv3;
    }
}
