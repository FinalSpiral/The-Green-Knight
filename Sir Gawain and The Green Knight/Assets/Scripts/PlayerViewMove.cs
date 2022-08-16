using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewMove : MonoBehaviour
{
    public Transform left, right;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            rb.velocity = new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0, 0);           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        rb.velocity = Vector3.zero;
    }
}
