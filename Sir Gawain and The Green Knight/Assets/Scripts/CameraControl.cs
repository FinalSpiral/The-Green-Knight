using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;

    public bool lockOn;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (lockOn && target!=null)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        }
    }
}
