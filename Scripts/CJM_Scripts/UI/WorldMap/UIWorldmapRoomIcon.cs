using System;
using System.Linq;
/*using UnityEditor.Search;*/
using UnityEngine;
using UnityEngine.UI;

public class UIWorldmapRoomIcon : MonoBehaviour
{
    [SerializeField] private Sprite verifiedRoomSpr;
    [SerializeField] private Sprite unverifiedRoomSpr;

    [SerializeField] private GameObject worldmapPortalObj;
    private GameObject[] worldmapPortals = new GameObject[4];
    private int myRoomID;

    public void Initialize(Vector2 position, int roomID)
    {
        myRoomID = roomID;        
        for(int i = 0; i < worldmapPortals.Length; i++)
        {
            if (RoomManager.Instance.GetRoomData(roomID).MyRoomData.MyEnableDir[i])
            {
                worldmapPortals[i] = Instantiate(worldmapPortalObj, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, i * 90.0f), transform);
                worldmapPortals[i].transform.localPosition = Vector3.zero;
            }
        }
        GetComponent<RectTransform>().localPosition = position;
    }

    public void ActivateIcons()
    {
        var roomData = RoomManager.Instance.GetRoomData(myRoomID);
        var myImage = GetComponent<Image>();
        if (roomData.IsMapClear)
        {
            myImage.sprite = verifiedRoomSpr;
            myImage.color = Color.white;
            SetAcvitePortals();
        }
        else if (roomData.ConnectedRoom.Where(i => i != null && i.IsMapClear == true).Count() > 0)
        {
            myImage.sprite = unverifiedRoomSpr;
            myImage.color = Color.white;
            SetAcvitePortals(GlobalEnums.SetActive.Inactive);
        }
        else
        {
            gameObject.SetActive(false);
            SetAcvitePortals(GlobalEnums.SetActive.Inactive);
        }
    }
    private void SetAcvitePortals(GlobalEnums.SetActive isActive = GlobalEnums.SetActive.Active)
    {
        for (int i = 0; i < worldmapPortals.Length; i++)
        {
            if(worldmapPortals[i] != null)
            {
                worldmapPortals[i].SetActive(Convert.ToBoolean(isActive));
            }
        }
    }
}
