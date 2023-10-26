using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // 카메라가 쫓아갈 대상
    /*public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;*/

    Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.15f;

    private void FixedUpdate()
    {
        Vector3 targetPos = target.position;

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        /*Follow();*/
    }

    /*void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = targetPosition;
    }*/
}
