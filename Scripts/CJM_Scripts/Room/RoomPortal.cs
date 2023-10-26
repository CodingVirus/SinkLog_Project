using UnityEngine;

public class RoomPortal : MonoBehaviour
{
    public LayerMask PlayerMask;
    private RoomPortal linkTo;
    private Vector2 jumpDelta;
    private int myParentRoomID;
    public int ParentRoomID { get => myParentRoomID;}

    public void SetJumpDelta(Vector2 jumpDirection)
    {
        jumpDelta = jumpDirection;
    }

    public void Initialize(RoomPortal targetObj, int parentRoomID)
    {
        linkTo = targetObj;
        myParentRoomID = parentRoomID;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((1 << collision.gameObject.layer & PlayerMask) != 0)
        {
            FadeEffectManager.Instance.JumpToNextRoom(collision.gameObject, 
                linkTo.transform.position + ((Vector3)jumpDelta), (Vector3)jumpDelta.normalized, linkTo.myParentRoomID, myParentRoomID);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & PlayerMask) != 0)
        {
            if (Time.timeScale != 0)
            {
                OnCollisionEnter2D(collision);
            }
        }
    }
}
