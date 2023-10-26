using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UIWorldmap : MonoBehaviour
{
    private IngameRoom[] roomDatas;
    private GameObject[] worldmapRooms;
    private GameObject worldmapRoomParent;

    [SerializeField] private GameObject emptyGameObject;
    [SerializeField] private GameObject worldmapRoomIcon;
    [SerializeField] private GameObject worldmapBackGround;
    [SerializeField] private GameObject worldmapPlayerIcon;

    private RoomManager roomManagerInst;
    private int roomIconPadding = 150;

    public void Initialize()
    {
        roomManagerInst = RoomManager.Instance;
        roomDatas = RoomManager.Instance.GetRoomData();
        worldmapRooms = new GameObject[roomDatas.Length];
        CreateBackground();
        CreateWorldMapIcons();
        CreatePlayerIcon();
    }

    public void Start()
    {
        Initialize();
    }
    
    private void CreateBackground()
    {
        worldmapBackGround = Instantiate(worldmapBackGround, transform);
        var myCanvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        worldmapBackGround.GetComponent<RectTransform>().sizeDelta = myCanvasRectTransform.sizeDelta;
        worldmapBackGround.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        worldmapBackGround.SetActive(false);
    }
    private void CreateWorldMapIcons()
    {
        worldmapRoomParent = Instantiate(emptyGameObject, transform);
        worldmapRoomParent.name = "WorldMapRooms";

        for (int i = 0; i < roomDatas.Length; i++)
        {
            worldmapRooms[i] = Instantiate(worldmapRoomIcon, worldmapRoomParent.transform);
            var xPos = (roomDatas[i].MyRoomData.x - (roomManagerInst.mapSizeX / 2)) * roomIconPadding;
            var yPos = (roomDatas[i].MyRoomData.y - (roomManagerInst.mapSizeY / 2)) * roomIconPadding;

            worldmapRooms[i].GetComponent<UIWorldmapRoomIcon>().Initialize(new Vector2(xPos, -yPos), roomDatas[i].MyRoomData.RoomID);
        }

        worldmapRoomParent.SetActive(false);
    }

    private void CreatePlayerIcon()
    {
        worldmapPlayerIcon = Instantiate(worldmapPlayerIcon, worldmapRoomParent.transform);
        worldmapPlayerIcon.SetActive(false);
    }

    private void Update()
    {
        var inputKey = SettingsManager.Instance.Inputkeys;
        if (inputKey.GetKeyPressed(GlobalEnums.InputKeys.WorldMapKey))
        {
            worldmapRoomParent.SetActive(!worldmapRoomParent.activeSelf);

            worldmapBackGround.SetActive(!worldmapBackGround.activeSelf);

            worldmapPlayerIcon.SetActive(!worldmapPlayerIcon.activeSelf);

            RefreshWorldMap();
        }

    }

    private void ShowWorldMap()
    {
        var minVectorValue = Vector3.zero;
        var maxVectorValue = Vector3.zero;

        for (int i = 0; i < roomDatas.Length; i++)
        {
            var roomData = RoomManager.Instance.GetRoomData(i);
            if (roomData.IsMapClear || roomData.ConnectedRoom.Where(i => i != null && i.IsMapClear == true).Count() > 0)
            {
                worldmapRooms[i].SetActive(true);
                minVectorValue = Vector2.Min(minVectorValue, worldmapRooms[i].transform.localPosition);
                maxVectorValue = Vector2.Max(maxVectorValue, worldmapRooms[i].transform.localPosition);
                if (RoomManager.Instance.IsPlayerInRoom(roomData.MyRoomData.RoomID))
                {
                    worldmapPlayerIcon.transform.localPosition = worldmapRooms[i].transform.localPosition;
                    worldmapPlayerIcon.GetComponent<UIWorldmapPlayerIcon>().BlinkPlayerLocation();
                }
            }
        }
        worldmapRoomParent.transform.localPosition = -((minVectorValue + maxVectorValue) / 2);
    }

    public void RefreshWorldMap()
    {
        if(worldmapRoomParent.activeSelf)
        {
            ShowWorldMap();
            for (int i = 0; i < roomDatas.Length; i++)
            {
                worldmapRooms[i].GetComponent<UIWorldmapRoomIcon>().ActivateIcons();
            }
        }
    }
}
