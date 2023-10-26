using UnityEngine;

//test

public class TempPlayer : MonoBehaviour
{
    private float speed = 6.0f;
    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += (Vector3)Vector2.up * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3)Vector2.left * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += (Vector3)Vector2.down * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3)Vector2.right * Time.deltaTime * speed;
        }
    }
}
