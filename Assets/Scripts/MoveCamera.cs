using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    Camera camera;
    float maxSize;
	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        
        float halfHeight = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10)).y - camera.ViewportToWorldPoint(new Vector3(0.5f, 0, 10)).y;
        float halfWidth = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10)).x - camera.ViewportToWorldPoint(new Vector3(0, 0.5f, 10)).x;
        if (halfWidth * 2 > 256 || halfHeight * 2 > 69.12f)
        {
            camera.orthographicSize = maxSize;
        }
        else
        {
            maxSize = camera.orthographicSize;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, halfWidth, 256 - halfWidth), Mathf.Clamp(transform.position.y, halfHeight, 69.12f-halfHeight), -10);
        
   
	}
    void moveXFunc()
    {
        
    }
    void moveYFunc()
    {

    }

}
