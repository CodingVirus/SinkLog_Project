using UnityEngine;

public class UIMinimapCamera : MonoBehaviour
{
    private void Update()
    {
        if(RoomManager.Instance.GetActiveRoom() != null)
        {
            if (RoomManager.Instance.GetActiveRoom().transform.position != transform.position)
            {
                transform.position = RoomManager.Instance.GetActiveRoom().transform.position;
            }
        }
    }
}
