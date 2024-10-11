using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public World world;
    public float scrollSpeed = 20f;
    Vector3 pos;
    private void Start()
    {
        pos = transform.position;
        pos.x = world.size / 2f;
        pos.y = -world.size / 2f;
        pos.z = -world.size;
    }
    void Update()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.z += scroll * scrollSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, 0, world.size);
        pos.y = Mathf.Clamp(pos.y, -world.size, 0);
        pos.z = Mathf.Clamp(pos.z, -world.size, -10f);
        transform.position = pos;

    }
}
