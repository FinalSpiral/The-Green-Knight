using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;   

    [SerializeField]
    private float timeToSwitch;

    private float elapsedTime;

    private bool lockOn;

    private Vector3 startPos, endPos;

    // Start is called before the first frame update
    void Start()
    {
        lockOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (lockOn)
            {
                transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
            }
            else if (!lockOn)
            {
                elapsedTime += Time.deltaTime;
                float percentOfTravel = elapsedTime / timeToSwitch;
                transform.position = Vector3.Lerp(startPos, endPos, percentOfTravel);
                if (percentOfTravel >= 1)
                {
                    lockOn = true;
                }
            }
        }
    }

    public void SwitchTo(Transform t)
    {
        startPos = transform.position;
        lockOn = false;
        target = t;
        elapsedTime = 0f;
        endPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }

}
