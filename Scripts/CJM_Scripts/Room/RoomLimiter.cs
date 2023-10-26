using UnityEngine;

public class RoomLimiter : MonoBehaviour
{
    private void Start()
    {
        transform.position = RoomManager.Instance.GetRoomData(0).gameObject.transform.position;
    }

    public void SetLimit(Vector3 position, Vector3 rightTop, Vector2 leftDown)
    {
        transform.position = position + new Vector3(0.5f, 0.5f);

        Vector2[] points = new Vector2[4];
        points[0] = new Vector2(rightTop.x, leftDown.y);
        points[1] = rightTop;
        points[2] = new Vector2(leftDown.x, rightTop.y);
        points[3] = leftDown;
    }
}
